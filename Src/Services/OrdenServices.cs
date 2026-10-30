using Microsoft.EntityFrameworkCore;
using TecnoFix.Src.Data;
using TecnoFix.Src.DTO.Orden;
using TecnoFix.Src.Model;
using TecnoFix.Src.Repositories.Intrefaces;
using TecnoFix.Src.Services.Interfaces;
using TecnoFix.Src.Utils;

namespace TecnoFix.Src.Services;

public class OrdenService(IOrdenRepository ordenRepository, IUsuarioRepository usuarioRepository,IBitacoraPdfService pdfService, IEmailSender emailSender) : IOrdenService
{
    private readonly IOrdenRepository _ordenRepository = ordenRepository;
    private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
    private readonly IBitacoraPdfService _pdfServices = pdfService;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task<RegistrarOrdenResponseDto?> RegistrarOrdenAsync(
        RegistrarOrdenRequestDto request, int idTecnico)
    {
        var rutNormalizado = ValidatorRut.ValidarRut(request.RutCliente);
        if (rutNormalizado is null)
            return new RegistrarOrdenResponseDto
            {
                Success = false,
                Message = "Rut de cliente no existe"
            };

        var cliente = await _usuarioRepository.ObtenerClientePorRutAsync(request.RutCliente);
        if (cliente is null)
        {
            return new RegistrarOrdenResponseDto
            {
                Message = "El rut existe" 
            };
        }
        var orden = new OrdenReparacion
        {
            IdCliente = cliente.Id,
            IdTecnico = idTecnico,
            TipoEquipo = request.TipoEquipo.Trim(),
            Marca = request.Marca.Trim(),
            Modelo = request.Modelo.Trim(),
            NumeroSerie = string.IsNullOrWhiteSpace(request.NumeroSerie) ? null : request.NumeroSerie.Trim(),
            DescripcionFalla = request.DescripcionFalla.Trim(),
            AccesoriosEntregados = string.IsNullOrWhiteSpace(request.AccesoriosEntregados) ? null : request.AccesoriosEntregados.Trim(),
            Estado = EstadoOrden.Recibida,
            FechaRecepcion = DateTime.UtcNow
        };
        //Guardamos con el repositories
        
        orden.Bitacora.Add(new BitacoraEvento
        {
            IdResponsable = idTecnico,
            EstadoAnterior = null,
            EstadoNuevo = EstadoOrden.Recibida,
            Comentario = "Orden registrada. Equipo recibido."
        });

        await _ordenRepository.AgregarAsync(orden);

        return new RegistrarOrdenResponseDto
        {
            Id = orden.Id,
            NumeroOrden = FormatearNumeroOrden(orden.Id),
            Estado = orden.Estado.ToString(),
            NombreCliente = cliente.Name,
            FechaRecepcion = orden.FechaRecepcion,
            Success =true
        };
    }
    public async Task<List<OrdenResumenDto>> ListarAsync()
{
    var ordenes = await _ordenRepository.ListarAsync();
    return ordenes.Select(o => new OrdenResumenDto
    {
        Id = o.Id,
        NumeroOrden = $"ORD-{o.Id:D6}",
        Cliente = o.Cliente.Name,
        Equipo = $"{o.TipoEquipo} {o.Marca} {o.Modelo}",
        Estado = o.Estado.ToString(),
        EstadosDisponibles = TransicionesEstado.Siguientes(o.Estado)
                                .Select(e => e.ToString()).ToList()
    }).ToList();
}

    public async Task<ActualizarEstadoResponseDto> ActualizarEstadoAsync(
        int idOrden, ActualizarEstadoRequestDto request, int idTecnico)
    {
        if (string.IsNullOrWhiteSpace(request.Comentario))
            return Fallo("El comentario es obligatorio.");

        if (!Enum.TryParse<EstadoOrden>(request.NuevoEstado, out var nuevo))
            return Fallo("El estado indicado no existe.");

        var orden = await _ordenRepository.ObtenerPorIdAsync(idOrden);
        if (orden == null)
            return Fallo("La orden no existe.");

        if (orden.Estado == EstadoOrden.Entregada)
            return Fallo("La orden ya fue entregada y no puede modificarse.");

        if (!TransicionesEstado.EsValida(orden.Estado, nuevo))
            return Fallo($"No se puede pasar de {orden.Estado} a {nuevo}.");

        var anterior = orden.Estado;
        orden.Estado = nuevo;
        orden.Bitacora.Add(new BitacoraEvento
        {
            FechaHora = DateTime.UtcNow,
            IdResponsable = idTecnico,
            EstadoAnterior = anterior,
            EstadoNuevo = nuevo,
            Comentario = request.Comentario.Trim()
        });

        try
        {
            // Cambio de estado + evento de bitácora se guardan juntos (una sola transacción).
            await _ordenRepository.GuardarCambiosAsync();
        }
        catch (Exception ex)
        {
            return Fallo("Error al guardar: " + (ex.InnerException?.Message ?? ex.Message));
        }

        var mensaje = "Estado actualizado correctamente.";

        // Al llegar a ListaParaRetiro se envía la bitácora en PDF al cliente.
        // El estado ya quedó guardado, por eso si el correo falla no se revierte nada.
        if (nuevo == EstadoOrden.ListaParaRetiro)
        {
            try
            {
                var pdf = _pdfServices.Generar(orden);
                await _emailSender.SendEmailWithAttachmentAsync(
                    orden.Cliente.Correo,
                    $"Tu equipo está listo para retiro - ORD-{orden.Id:D6}",
                    "<p>Tu equipo está listo para retiro. Adjuntamos la bitácora de la reparación.</p>",
                    pdf,
                    $"Bitacora-ORD-{orden.Id:D6}.pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo de la bitácora: " + ex.Message);
                mensaje += " No se pudo enviar el correo al cliente.";
            }
        }

        return new ActualizarEstadoResponseDto
        {
            Success = true,
            Mensaje = mensaje,
            EstadoActual = nuevo.ToString()
        };
    }

    private static ActualizarEstadoResponseDto Fallo(string mensaje) =>
        new() { Success = false, Mensaje = mensaje };

    // Único lugar donde se define el formato del número de orden (ambigüedad 6 del ERS, pendiente).
    private static string FormatearNumeroOrden(int id) => $"ORD-{id:D6}";
}
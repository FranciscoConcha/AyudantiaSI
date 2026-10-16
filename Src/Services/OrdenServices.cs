using Microsoft.EntityFrameworkCore;
using TecnoFix.Src.Data;
using TecnoFix.Src.DTO.Orden;
using TecnoFix.Src.Model;
using TecnoFix.Src.Repositories.Intrefaces;
using TecnoFix.Src.Services.Interfaces;
using TecnoFix.Src.Utils;

namespace TecnoFix.Src.Services;

public class OrdenService(IOrdenRepository ordenRepository, IUsuarioRepository usuarioRepository) : IOrdenService
{
    private readonly IOrdenRepository _ordenRepository = ordenRepository;
    private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

    public async Task<RegistrarOrdenResponseDto?> RegistrarOrdenAsync(
        RegistrarOrdenRequestDto request, int idTecnico)
    {
        var rutNormalizado = ValidatorRut.ValidarRut(request.RutCliente);
        if (rutNormalizado is null)
            return new RegistrarOrdenResponseDto
            {
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
            FechaRecepcion = orden.FechaRecepcion
        };
    }

    // Único lugar donde se define el formato del número de orden (ambigüedad 6 del ERS, pendiente).
    private static string FormatearNumeroOrden(int id) => $"ORD-{id:D6}";
}
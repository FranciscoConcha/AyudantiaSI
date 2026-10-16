using System.ComponentModel.DataAnnotations;

namespace TecnoFix.Src.DTO.Orden;

public class RegistrarOrdenRequestDto
{
    [Required(ErrorMessage = "Debe completar el campo <<RUT del cliente>>")]
    public string RutCliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe completar el campo <<Tipo de equipo>>")]
    public string TipoEquipo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe completar el campo <<Marca>>")]
    public string Marca { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe completar el campo <<Modelo>>")]
    public string Modelo { get; set; } = string.Empty;

    public string? NumeroSerie { get; set; }   // opcional

    [Required(ErrorMessage = "Debe completar el campo <<Descripción de la falla>>")]
    public string DescripcionFalla { get; set; } = string.Empty;

    public string? AccesoriosEntregados { get; set; }   // opcional
}

public class RegistrarOrdenResponseDto
{
    public int Id { get; set; }
    public string NumeroOrden { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string NombreCliente { get; set; } = string.Empty;
    public DateTime FechaRecepcion { get; set; }
}
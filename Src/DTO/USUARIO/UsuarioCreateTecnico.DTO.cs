using System.ComponentModel.DataAnnotations;

namespace TecnoFix.Src.DTO.Usuario;
// Datos de solicitud
public class RegistrarTecnicoRequestDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La especialidad es obligatoria.")]
    public string Especialidad { get; set; } = string.Empty;
}
//Datos de respuesta.
public class RegistrarTecnicoResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    public string Mensaje { get; set; } = "Técnico registrado. La contraseña temporal fue enviada a su correo.";
    public bool Success {get;set;}
}
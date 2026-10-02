namespace TecnoFix.Src.DTO.Usuario;
/// <summary>
/// Representa la solicitud de inicio de sesión de un usuario.
/// </summary>
public class LoginRequestDto
{
    public string Correo { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
/// <summary>
/// Representa la respuesta de un intento de inicio de sesión.
/// </summary>
public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;    
    public string Name { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
namespace TecnoFix.Src.DTO.Usuario;

public class LoginRequestDto
{
    public string Correo { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;    
    public string Name { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
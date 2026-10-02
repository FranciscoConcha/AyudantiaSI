using Microsoft.AspNetCore.Mvc;
using TecnoFix.Src.DTO.Usuario;
using TecnoFix.Src.Services;

namespace TecnoFix.Src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    // POST: http://localhost:5321/api/auth/login
    // Generando un token JWT para el usuario autenticado 
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Correo) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { mensaje = "Debe completar correo y contraseña" });
        
        // Llamando al servicio de autenticación para validar las credenciales del usuario
        var resultado = await _authService.LoginAsync(request);

        if (resultado is null || (string.IsNullOrEmpty(resultado.Name) && string.IsNullOrEmpty(resultado.Rol)))
            return Unauthorized(new { mensaje = resultado?.Message ?? "Correo o contraseña incorrectos" });

        Response.Cookies.Append("access_token", resultado.Token, new CookieOptions
        {
            HttpOnly = true, 
            Secure = true,           
            SameSite = SameSiteMode.None,  
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        });

        return Ok(
            new { usuario = resultado.Name,
            rol = resultado.Rol
        }); 
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegistrarClienteResponseDto>> RegistrarCliente(
    [FromBody] RegistrarClienteRequestDto request)
    {
        var resultado = await _authService.RegistrarClienteAsync(request);

        if (resultado is null)
            return BadRequest(new { mensaje = "Error al registrar el cliente" }
            );

        return Ok(resultado);
    }

    [HttpHead("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("access_token");
        return Ok(new { mensaje = "Sesión cerrada" });
    }
}
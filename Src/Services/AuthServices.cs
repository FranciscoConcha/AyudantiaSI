using Microsoft.EntityFrameworkCore;
using TecnoFix.Src.Data;
using TecnoFix.Src.DTO.Usuario;
using TecnoFix.Src.Utils;

using TecnoFix.Src.Services.Interfaces;
using TecnoFix.Src.Repositories.Intrefaces;
using TecnoFix.Src.Repositories;

namespace TecnoFix.Src.Services;
/// <summary>
/// Implementa los servicios de autenticación y registro de usuarios.
/// </summary>
/// <param name="context">Base de datos</param>
/// <param name="configuration">Configuración de la aplicación</param>
/// <param name="emailSender">Servicio para envío de correos electrónicos</param>
public class AuthService(IUsuarioRepository usuarioRepository, IRolRepository rolRepository, IConfiguration configuration, IEmailSender emailSender) : IAuthService
{
    // Inyección de dependencias para el contexto de la base de datos, 
    // la configuración y el servicio de correo electrónico

    private readonly IRolRepository _rolRepository = rolRepository;
    private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
    private readonly IConfiguration _configuration = configuration;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var usuario = await _usuarioRepository.ObtenerPorCorreoAsync(request.Correo);
            // Clase usuario || null;
                                //  PANCHO123 => FASFASF   == WEKFNWE
        if (usuario is null  || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
            return new LoginResponseDto
            {
                Name = string.Empty,
                Rol = string.Empty,
                Message = "Correo o contraseña incorrectos"
            };

        return new LoginResponseDto
        {
            Token = new GenerateTokenClass(_configuration).GenerarToken(usuario),
            Name = usuario.Name,
            Rol = usuario.RolUsuario?.Name ?? string.Empty,
            Message = "Login successful"
        };
    }

    public async Task<RegistrarClienteResponseDto> RegistrarClienteAsync(RegistrarClienteRequestDto request)
    {
        
        var CorreoNormalizado = request.Correo.Trim().ToLowerInvariant();
        var rutNormalizado = ValidatorRut.ValidarRut(request.Rut);
        if (rutNormalizado is null)
            return new RegistrarClienteResponseDto { Mensaje = "El RUT ingresado no es válido." };

        var rolCliente = await _rolRepository.ObtenerPorNombreAsync("Cliente");
        if (rolCliente is null)
            return new RegistrarClienteResponseDto { Mensaje = "No se encontró el rol de Cliente en la base de datos." };

        var usuarioExistenteCorreo = await _usuarioRepository.ExisteCorreoAsync(CorreoNormalizado);
        var usuarioExistenteRut = await _usuarioRepository.ExisteRutAsync(rutNormalizado);
        if (!usuarioExistenteCorreo && !usuarioExistenteRut)
            return new RegistrarClienteResponseDto { Mensaje = "El correo o RUT ya está registrado." };
        var passwordTemporal = PasswordGenerator.GenerateRandomPassword();
        
    
        try
        {
            
            var cuerpoHtml = $"""
                <div style="font-family: Arial, sans-serif; font-size: 14px; color: #333; max-width: 480px;">
                <h2 style="color: #1d4ed8;">Bienvenido a TecnoFix, {request.Name}</h2>
                <p>Tu cuenta fue creada exitosamente. Esta es tu contraseña temporal:</p>
                <p style="font-size: 18px; font-weight: bold; letter-spacing: 1px;">{passwordTemporal}</p>
                <p>Te recomendamos cambiarla después de tu primer inicio de sesión.</p>
                <hr style="border: none; border-top: 1px solid #ddd; margin: 20px 0;">
                <p style="font-size: 12px; color: #888;">Este es un correo automático, por favor no respondas.</p>
                </div>
            """;
            await _emailSender.SendEmailAsync(request.Correo.Trim(), "Tu contraseña temporal - TecnoFix", cuerpoHtml);
            
        }
        catch (Exception ex)
        {
            return new RegistrarClienteResponseDto
            {
                Id = 0,
                Name = "",
                Correo = "",
                Mensaje = $"Cliente no registrado, falló el envío del correo: {ex.Message}"
            };
        }
        var nuevoUsuario = new Model.Usuario
            {
                Name = request.Name,
                Correo = CorreoNormalizado,
                Rut = rutNormalizado,
                Telefono = request.Telefono,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordTemporal),
                IdRol = rolCliente.Id
            };

            await _usuarioRepository.AgregarAsync(nuevoUsuario);
            return new RegistrarClienteResponseDto
            {
                Id = nuevoUsuario.Id,
                Name = nuevoUsuario.Name,
                Correo = nuevoUsuario.Correo,
                Mensaje = "Cliente registrado. Revisa tu correo para la contraseña temporal."
            };
    }

    public async Task<RegistrarTecnicoResponseDto> RegistrarTecnicoAsync(RegistrarTecnicoRequestDto request)
    {
        try
        {  
            var CorreoNormalizado = request.Correo.Trim().ToLowerInvariant();
        
            var rolCliente = await _rolRepository.ObtenerPorNombreAsync("Tecnico");
            if (rolCliente is null)
                return new RegistrarTecnicoResponseDto { Mensaje = "No se encontró el rol de Cliente en la base de datos." };

            var usuarioExistenteCorreo = await _usuarioRepository.ExisteCorreoAsync(CorreoNormalizado);
            if (!usuarioExistenteCorreo )
                return new RegistrarTecnicoResponseDto { Mensaje = "El correo ya está registrado." };
            var passwordTemporal = PasswordGenerator.GenerateRandomPassword();
            try
            {
                
                var cuerpoHtml = $"""
                    <div style="font-family: Arial, sans-serif; font-size: 14px; color: #333; max-width: 480px;">
                    <h2 style="color: #1d4ed8;">Bienvenido a TecnoFix, {request.Name}</h2>
                    <p>Tu cuenta fue creada exitosamente. Esta es tu contraseña temporal:</p>
                    <p style="font-size: 18px; font-weight: bold; letter-spacing: 1px;">{passwordTemporal}</p>
                    <p>Te recomendamos cambiarla después de tu primer inicio de sesión.</p>
                    <hr style="border: none; border-top: 1px solid #ddd; margin: 20px 0;">
                    <p style="font-size: 12px; color: #888;">Este es un correo automático, por favor no respondas.</p>
                    </div>
                """;
                await _emailSender.SendEmailAsync(request.Correo.Trim(), "Tu contraseña temporal - TecnoFix", cuerpoHtml);
                
            }
            catch (Exception ex)
            {
                return new RegistrarTecnicoResponseDto
                {
                    Id = 0,
                    Name = "",
                    Correo = "",
                    Mensaje = $"Cliente no registrado, falló el envío del correo: {ex.Message}",
                    Success=false
                };
            }
            var nuevoUsuario = new Model.Usuario
                {
                    Name = request.Name,
                    Correo = CorreoNormalizado,                
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordTemporal),
                    IdRol = rolCliente.Id
                };

                await _usuarioRepository.AgregarAsync(nuevoUsuario);
                return new RegistrarTecnicoResponseDto
                {
                    Id = nuevoUsuario.Id,
                    Name = nuevoUsuario.Name,
                    Correo = nuevoUsuario.Correo,
                    Mensaje = "Cliente registrado. Revisa tu correo para la contraseña temporal.",
                    Success = true
                };

            
        }catch(Exception ex)
        {
            return new RegistrarTecnicoResponseDto
            {
                Id = 0,
                Name = "",
                Correo = "",
                Especialidad = "",
                Mensaje = "Error en el servicio "+ ex.Message,
                Success =false
            };
        }
        
    }
}
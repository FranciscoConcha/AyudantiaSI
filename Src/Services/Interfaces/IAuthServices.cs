using TecnoFix.Src.DTO.Usuario;
namespace TecnoFix.Src.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    Task<RegistrarClienteResponseDto> RegistrarClienteAsync(RegistrarClienteRequestDto request);
}
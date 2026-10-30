using TecnoFix.Src.DTO.Orden;

namespace TecnoFix.Src.Services.Interfaces;

public interface IOrdenService
{
    Task<RegistrarOrdenResponseDto?> RegistrarOrdenAsync(
    RegistrarOrdenRequestDto request, int idTecnico);
    Task<ActualizarEstadoResponseDto> ActualizarEstadoAsync(int idOrden, ActualizarEstadoRequestDto request, int idTecnico);
    Task<List<OrdenResumenDto>> ListarAsync();
}
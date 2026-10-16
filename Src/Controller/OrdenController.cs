using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TecnoFix.Src.DTO.Orden;
using TecnoFix.Src.Services.Interfaces;

namespace TecnoFix.Src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdenController(IOrdenService ordenService) : ControllerBase
{
    private readonly IOrdenService _ordenService = ordenService;

    [Authorize(Roles = "Tecnico")]
    [HttpPost]
    public async Task<ActionResult<RegistrarOrdenResponseDto>> RegistrarOrden(
        [FromBody] RegistrarOrdenRequestDto request)
    {
        var idTecnico = ObtenerIdUsuarioActual();
        if (idTecnico is null)
            return Unauthorized(new { mensaje = "No se pudo identificar al usuario." });

        var responseDto = await _ordenService.RegistrarOrdenAsync(request, idTecnico.Value);

        if (responseDto is null  )
            return BadRequest(new { mensaje = "Error en el servidor"  });
        if(!responseDto.Succes)
        {
            return BadRequest(new {mensaje = responseDto.Message});
        }
        return Ok(responseDto);
    }

    // El id del usuario viene en el JWT (claim "sub"). Según cómo ASP.NET mapee los claims,
    // puede llegar como NameIdentifier o como "sub": revisamos ambos.
    private int? ObtenerIdUsuarioActual()
    {
        var valor = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return int.TryParse(valor, out var id) ? id : null;
    }
}
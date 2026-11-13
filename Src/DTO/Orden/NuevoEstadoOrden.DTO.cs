using System.ComponentModel.DataAnnotations;

public class ActualizarEstadoRequestDto
{
    [Required] 
    public string NuevoEstado { get; set; } = null!;
    // ERS ORD-004: el comentario es obligatorio (sin largo mínimo)
    [Required(ErrorMessage = "Debe ingresar un comentario del avance")]
    public string Comentario { get; set; } = null!;
}

public class ActualizarEstadoResponseDto
{
    public bool Success { get; set; }
    public string Mensaje { get; set; } = null!;
    public string? EstadoActual { get; set; }
}

public class OrdenResumenDto
{
    public int Id { get; set; }
    public string NumeroOrden { get; set; } = null!;
    public string Cliente { get; set; } = null!;
    public string Equipo { get; set; } = null!;
    public string Estado { get; set; } = null!;
    public List<string> EstadosDisponibles { get; set; } = new();
}
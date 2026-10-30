using System.ComponentModel.DataAnnotations;

public class ActualizarEstadoRequestDto
{
    [Required] 
    public string NuevoEstado { get; set; } = null!;
    [Required, MinLength(5)] 
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
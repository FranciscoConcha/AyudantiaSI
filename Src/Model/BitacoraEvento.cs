namespace TecnoFix.Src.Model;
//
public class BitacoraEvento
{
    public int Id { get; set; }

    public int IdOrden { get; set; }
    public OrdenReparacion Orden { get; set; } = default!;
    // Vitacora
    public DateTime FechaHora { get; set; } = DateTime.UtcNow;

    public int IdResponsable { get; set; }
    public Usuario Responsable { get; set; } = default!;

    public EstadoOrden? EstadoAnterior { get; set; }   // null en el primer evento (no hay estado previo)
    public EstadoOrden EstadoNuevo { get; set; }

    public string Comentario { get; set; } = string.Empty;
}
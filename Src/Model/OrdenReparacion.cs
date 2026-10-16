namespace TecnoFix.Src.Model;

public class OrdenReparacion
{
    //Id autoincrementable
    public int Id { get; set; } 

    public int IdCliente { get; set; }
    public Usuario Cliente { get; set; } = default!;

    public int IdTecnico { get; set; }
    public Usuario Tecnico { get; set; } = default!;

    public string TipoEquipo { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string? NumeroSerie { get; set; }
    public string DescripcionFalla { get; set; } = string.Empty;
    public string? AccesoriosEntregados { get; set; }

    public EstadoOrden Estado { get; set; } = EstadoOrden.Recibida;
    public DateTime FechaRecepcion { get; set; } = DateTime.UtcNow;

    public ICollection<BitacoraEvento> Bitacora { get; set; } = new List<BitacoraEvento>();

    // Diagnóstico, costos y datos de entrega se agregan en ORD-002 y ORD-006.
}
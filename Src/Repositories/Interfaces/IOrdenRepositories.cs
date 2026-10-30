using TecnoFix.Src.Model;

public interface IOrdenRepository
{
    Task AgregarAsync(OrdenReparacion orden);
    //
    Task<OrdenReparacion?> ObtenerPorIdAsync(int id);
    Task<List<OrdenReparacion>> ListarAsync();
    Task GuardarCambiosAsync();
}
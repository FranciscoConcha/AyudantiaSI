using TecnoFix.Src.Model;

public interface IOrdenRepository
{
    Task AgregarAsync(OrdenReparacion orden);
}
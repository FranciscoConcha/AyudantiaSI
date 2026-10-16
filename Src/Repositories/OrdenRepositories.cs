using TecnoFix.Src.Data;
using TecnoFix.Src.Model;

namespace TecnoFix.Src.Repositories;
/// <summary>
/// Clase que representa las funciones repetitivas de orden 
/// </summary>
/// <param name="context"></param>
public class OrdenRepository(TecnoFixDbContext context) : IOrdenRepository
{
    public async Task AgregarAsync(OrdenReparacion orden)
    {
        context.Ordenes.Add(orden);
        await context.SaveChangesAsync();   // guarda orden + bitácora en una sola transacción
    }
}
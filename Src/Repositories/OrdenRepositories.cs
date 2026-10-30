using TecnoFix.Src.Data;
using TecnoFix.Src.Model;
using Microsoft.EntityFrameworkCore;

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
    public async Task<OrdenReparacion?> ObtenerPorIdAsync(int id) =>
        await context.Ordenes
            .Include(o => o.Cliente)
            .Include(o => o.Bitacora).ThenInclude(b => b.Responsable)
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<List<OrdenReparacion>> ListarAsync() =>
        await context.Ordenes
            .Include(o => o.Cliente)
            .OrderByDescending(o => o.FechaRecepcion)
            .ToListAsync();

    public async Task GuardarCambiosAsync() => await context.SaveChangesAsync();
}
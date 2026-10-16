using TecnoFix.Src.Data;
using TecnoFix.Src.Model;
using TecnoFix.Src.Repositories.Intrefaces;
using Microsoft.EntityFrameworkCore;

namespace TecnoFix.Src.Repositories;
/// <summary>
/// Clase que contiene las funciones más repetidas para la clase de Rol
/// </summary>
/// <param name="context"></param>
public class RolRepository(TecnoFixDbContext context) : IRolRepository
{
    private readonly TecnoFixDbContext _context = context;
    public Task<Rol?> ObtenerPorNombreAsync(string nombre)
    {
        return _context.Roles.FirstOrDefaultAsync(r => r.Name == nombre);
    }
}
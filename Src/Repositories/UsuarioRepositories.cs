using Microsoft.EntityFrameworkCore;
using TecnoFix.Src.Data;
using TecnoFix.Src.Model;
using TecnoFix.Src.Repositories.Intrefaces;

namespace TecnoFix.Src.Repositories;
/// <summary>
/// Clase que contiene las solicitudes al backen requetidas o que se podrian repetir en algun momento.
/// </summary>
/// <param name="context"></param>
public class UsuarioRepository(TecnoFixDbContext context) : IUsuarioRepository
{
    private readonly TecnoFixDbContext _context = context;

    public Task<Usuario?> ObtenerPorCorreoAsync(string correo)
    {
        return _context.Usuarios.Include(u => u.RolUsuario).FirstOrDefaultAsync(u => u.Correo == correo.ToLower());
    }
        

    public Task<Usuario?> ObtenerClientePorRutAsync(string rut)
    {
        return _context.Usuarios.FirstOrDefaultAsync(u => u.Rut == rut && u.RolUsuario != null && u.RolUsuario.Name == "Cliente");
    }

    public Task<bool> ExisteCorreoAsync(string correo)
    {
        return _context.Usuarios.AnyAsync(u => u.Correo == correo.ToLower());   
    }
        

    public Task<bool> ExisteRutAsync(string rut)
    {
        return _context.Usuarios.AnyAsync(u => u.Rut == rut);
    }
        
    public async Task AgregarAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
    }
}
using TecnoFix.Src.Model;

namespace TecnoFix.Src.Repositories.Intrefaces;
public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorCorreoAsync(string correo);
    Task<Usuario?> ObtenerClientePorRutAsync(string rut);
    Task<bool> ExisteCorreoAsync(string correo);
    Task<bool> ExisteRutAsync(string rut);
    Task AgregarAsync(Usuario usuario);
}
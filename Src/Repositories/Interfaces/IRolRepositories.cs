using TecnoFix.Src.Model;
namespace TecnoFix.Src.Repositories.Intrefaces;
public interface IRolRepository
{
    Task<Rol?> ObtenerPorNombreAsync(string nombre);
}
namespace TecnoFix.Src.Model;
/// <summary>
/// Representa un rol de usuario en el sistema.
/// </summary>
public class Rol {
    
    public int Id {get;set;}
    
    public string Name{get;set;} = string.Empty;
    
    // lista de usuarios que pertenecen a este rol
    public ICollection<Usuario> Usuarios { get; set; } = [];

}
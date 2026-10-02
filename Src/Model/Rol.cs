namespace TecnoFix.Src.Model;
public class Rol {
    
    public int Id {get;set;}
    
    public string Name{get;set;} = string.Empty;
    
    // lista de usuarios que pertenecen a este rol
    public ICollection<Usuario> Usuarios { get; set; } = [];

}
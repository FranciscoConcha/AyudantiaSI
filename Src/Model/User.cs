namespace TecnoFix.Src.Model;

public class Usuario {
    public int Id {get;set;}
    
    public string Name{get;set;} = string.Empty;
    
    public string Correo {get;set;} = string.Empty;
    // nuevos atributos agregadas para el usuario
    public string Rut {get;set;} = string.Empty;
    public string Telefono {get;set;} = string.Empty;
    
    public string PasswordHash {get;set;} = string.Empty;
    // Clave foranea del rol del usuario
    public int IdRol {get;set;}

    // Propiedad de navegación para acceder al rol del usuario
    public Rol? RolUsuario {get;set;}
}
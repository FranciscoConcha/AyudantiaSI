// Data/DbSeeder.cs
using Microsoft.EntityFrameworkCore;
using TecnoFix.Src.Model;

namespace TecnoFix.Src.Data;
/// <summary>
/// Clase estática que se encarga de sembrar datos iniciales en la base de datos.
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Siembra los datos iniciales en la base de datos si no existen.
    /// </summary>
    /// <param name="context">Base de datos</param>
    /// <returns></returns>
    public static async Task SeedAsync(TecnoFixDbContext context)
    {
        if (!await context.Roles.AnyAsync())
        {
            context.Roles.AddRange(
                new Rol { Name = "Administrador" },
                new Rol { Name = "Cliente" },
                new Rol { Name = "Tecnico" }
            );
            await context.SaveChangesAsync();
        }

        var correoAdmin = "admin@tecnofix.cl";
        
        var yaExiste = await context.Usuarios.AnyAsync(u => u.Correo == correoAdmin);
        if (!yaExiste)
        {
            var rolAdmin = await context.Roles.FirstAsync(r => r.Name == "Administrador");

            context.Usuarios.Add(new Usuario
            {
                Name = "Admin",
                Correo = correoAdmin,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"), // se calcula en runtime, no en el modelo
                IdRol = rolAdmin.Id
            });

            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Crea los usuarios de prueba que usan los tests E2E (Playwright) y las pruebas de carga (k6).
    /// Solo se ejecuta en desarrollo (ver Program.cs). Contraseña de ambos: Prueba123.
    /// Si ya existen, no hace nada.
    /// </summary>
    /// <param name="context">Base de datos</param>
    public static async Task SeedDatosPruebaAsync(TecnoFixDbContext context)
    {
        var rolTecnico = await context.Roles.FirstAsync(r => r.Name == "Tecnico");
        var rolCliente = await context.Roles.FirstAsync(r => r.Name == "Cliente");

        // Técnico de prueba
        if (!await context.Usuarios.AnyAsync(u => u.Correo == "tecnico.prueba@tecnofix.cl"))
        {
            context.Usuarios.Add(new Usuario
            {
                Name = "Tecnico Prueba",
                Correo = "tecnico.prueba@tecnofix.cl",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Prueba123"),
                IdRol = rolTecnico.Id,
                Especialidad = "Notebooks"
            });
        }

        // Cliente de prueba: las órdenes de los tests E2E se registran con su RUT
        var rutCliente = "192837464"; // 19.283.746-4
        if (!await context.Usuarios.AnyAsync(u => u.Correo == "cliente.prueba@tecnofix.cl" || u.Rut == rutCliente))
        {
            context.Usuarios.Add(new Usuario
            {
                Name = "Cliente Prueba",
                Correo = "cliente.prueba@tecnofix.cl",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Prueba123"),
                IdRol = rolCliente.Id,
                Rut = rutCliente,
                Telefono = "+56911111111"
            });
        }

        await context.SaveChangesAsync();
    }
}

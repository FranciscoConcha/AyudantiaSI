using Microsoft.EntityFrameworkCore;
using TecnoFix.Src.Model;

namespace TecnoFix.Src.Data;

public class TecnoFixDbContext : DbContext
{
    public TecnoFixDbContext(DbContextOptions<TecnoFixDbContext> options) : base(options) { }
    // Creaciones de las tablas
    public DbSet<Usuario> Usuarios => Set<Usuario>(); 
    public DbSet<Rol> Roles => Set<Rol>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rol>(e =>
        {
            e.ToTable("Roles");
            e.HasKey(r => r.Id);
            e.Property(r => r.Name).HasMaxLength(30).IsRequired();
        });

        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("Usuarios");
            e.HasKey(u => u.Id);

            e.Property(u => u.Name).HasMaxLength(150).IsRequired();
            e.Property(u => u.Correo).HasMaxLength(256).IsRequired();
            e.Property(u => u.PasswordHash).IsRequired();
            e.Property(u => u.Rut).HasMaxLength(10);
            
            e.HasIndex(u => u.Correo).IsUnique();
            e.HasIndex(u => u.Rut).IsUnique().HasFilter("\"Rut\" IS NOT NULL");

            e.HasOne(u => u.RolUsuario)
             .WithMany(r => r.Usuarios)
             .HasForeignKey(u => u.IdRol)
             .OnDelete(DeleteBehavior.Restrict);      
        });

        base.OnModelCreating(modelBuilder);
    }
}
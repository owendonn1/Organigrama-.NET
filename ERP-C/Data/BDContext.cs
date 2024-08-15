using ERP_C.Models;
using Humanizer.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace ERP_C.Data
{
    public class BDContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {

        public BDContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser<int>>().ToTable("Personas");
            modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("PersonasRoles");
            modelBuilder.Entity<Gasto>().Property(x => x.Monto).HasColumnType("decimal").HasPrecision(17, 2);
            modelBuilder.Entity<Telefono>()
                .HasOne<TipoTelefono>()
                .WithMany()
                .HasForeignKey(p => p.TipoTelefonoId);
            #region unique
            modelBuilder.Entity<Empresa>().HasIndex(e => e.Nombre).IsUnique();
            modelBuilder.Entity<Gerencia>().HasIndex(g => g.Nombre).IsUnique();
            modelBuilder.Entity<Posicion>().HasIndex(p => p.Nombre).IsUnique();
            modelBuilder.Entity<TipoTelefono>().HasIndex(t => t.Nombre).IsUnique();
            #endregion



        }


        public DbSet<CentroDeCosto> CentroDeCostos { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Foto> Fotos { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<Gerencia> Gerencias  { get; set; }

        public DbSet<Persona> Personas { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Posicion> Posiciones  { get; set; }
        public DbSet<Telefono> Telefonos  { get; set; }
        public DbSet<TipoTelefono> TipoTelefonos { get; set; }
      

        
    }
}

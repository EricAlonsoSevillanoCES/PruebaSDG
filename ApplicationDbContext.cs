using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebApiPoblacion.Models;
using Microsoft.EntityFrameworkCore;
namespace WebApiTiempo
{
    public class ApplicationDbContext: DbContext
    {
        // Clase para crear las migraciones de la base de datos
        public ApplicationDbContext(DbContextOptions options): base(options)
        {  
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasKey(c => c.Id); // Clave primaria
            modelBuilder.Entity<Country>()
                .OwnsOne(c => c.name, n =>
                {
                    n.Property(name => name.common).HasColumnName("NameCommon");
                    n.Property(name => name.official).HasColumnName("NameOfficial");
                    n.HasIndex(name => name.common).IsUnique();
                }); // Propiedad Name compuesta por dos valores
        }

        public DbSet<Country> Countries { get; set; }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using webApiRegistros.Entidades;

namespace webApiRegistros
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Registro> Registros { get; set; }
        public DbSet<Objeto> Objetos { get; set; }

    }
}
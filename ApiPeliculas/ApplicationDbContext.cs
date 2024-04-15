using ApiPeliculas.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        DbSet<Genero> Generos { get; set; }
       
    }
}

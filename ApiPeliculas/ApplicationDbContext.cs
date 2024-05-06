﻿using ApiPeliculas.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PeliculaActor>()
                .HasKey(x => new { x.ActorId, x.PeliculaId });

            modelBuilder.Entity<PeliculaGenero>()
                .HasKey(x => new { x.PeliculaId, x.GeneroId });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Pelicula> Peliculas { get; set;}
        public DbSet<PeliculaActor> PeliculasActores { get;set; }
        public DbSet<PeliculaGenero> PeliculasGeneros { get; set; }

    }
}


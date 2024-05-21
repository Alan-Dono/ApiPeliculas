using ApiPeliculas.DTOs;
using ApiPeliculas.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;

namespace ApiPeliculas.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap(); // Desde/Hacia y reversa
            CreateMap<GeneroCreacionDTO, Genero>();

            CreateMap<IdentityUser, UsuarioDTO>();

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCrecionDTO, Actor>()
                .ForMember(x => x.Foto, opciones => opciones.Ignore());
            CreateMap<ActorPatchDTO, Actor>().ReverseMap();

            CreateMap<Pelicula, PeliculaDTO>().ReverseMap();
            CreateMap<PeliculaCreacionDTO, Pelicula>()
                .ForMember(x => x.Poster, opciones => opciones.Ignore())
                .ForMember(x => x.PeliculasGeneros, options => options.MapFrom(MapPeliculasGeneros))
                .ForMember(x => x.PeliculasActores, opciones => opciones.MapFrom(MapPeliculasActores));
            CreateMap<PeliculaPatchDTO, Pelicula>().ReverseMap();
            CreateMap<Pelicula, PeliculaDetallerDTO>()
                .ForMember(x => x.Generos, options => options.MapFrom(MapPeliculasGeneros))
                .ForMember(x => x.Actores, options => options.MapFrom(MapPeliculasActores));

            CreateMap<SalaDeCine, SalaDeCineDTO>()
                .ForMember(x => x.Latitud, x => x.MapFrom(y => y.Ubicacion.Y))
                .ForMember(x => x.Longitud , x => x.MapFrom(y => y.Ubicacion.X));


            CreateMap<SalaDeCineDTO, SalaDeCine>()
                .ForMember(x => x.Ubicacion, x => x.MapFrom(
                    y => geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));

            CreateMap<SalaDeCineCreacionDTO, SalaDeCine>()
                .ForMember(x => x.Ubicacion, x => x.MapFrom(
                    y => geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));

            CreateMap<Review, ReviewDTO>()
                .ForMember(x => x.NombreUsuario, x => x.MapFrom(y => y.Usuario.UserName));

            CreateMap<ReviewDTO, Review>();
            CreateMap<ReviewCreacionDTO, Review>();
        }

        private List<ActorPeliculaDetalleDTO> MapPeliculasActores(Pelicula pelicula, PeliculaDetallerDTO peliculaDetallerDTO)
        {
            var resultado = new List<ActorPeliculaDetalleDTO>();
            if (pelicula.PeliculasActores == null) { return resultado; }
            foreach(var actorPelicula in pelicula.PeliculasActores)
            {
                resultado.Add(new ActorPeliculaDetalleDTO
                {
                    ActorId = actorPelicula.ActorId,
                    Personaje = actorPelicula.Personje,
                    NombrePersona = actorPelicula.Actor.Nombre
                });
            }
            return resultado;
        }

        private List<GeneroDTO> MapPeliculasGeneros(Pelicula pelicula, PeliculaDetallerDTO peliculaDetallerDTO)
        {
            var resultado = new List<GeneroDTO>();
            if (pelicula.PeliculasGeneros == null) { return resultado;}
            foreach(var generoPelicula in pelicula.PeliculasGeneros)
            {
                resultado.Add(new GeneroDTO { Id = generoPelicula.GeneroId, Nombre = generoPelicula.Genero.Nombre});    
            }
            return resultado;
        }

        private List<PeliculaGenero> MapPeliculasGeneros(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resultado = new List<PeliculaGenero>();
            if (peliculaCreacionDTO.GenerosIDs == null)
            {
                return resultado;
            }
            foreach(var id in peliculaCreacionDTO.GenerosIDs)
            {
                resultado.Add(new PeliculaGenero { GeneroId = id });
            }
            return resultado;
        }

        private List<PeliculaActor> MapPeliculasActores(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resultado = new List<PeliculaActor>();
            if (peliculaCreacionDTO.Actores == null)
            {
                return resultado;
            }
            foreach(var actor in peliculaCreacionDTO.Actores)
            {
                resultado.Add(new PeliculaActor { ActorId = actor.ActorId, Personje = actor.Personaje});
            }
            return resultado;
        }
    }
}

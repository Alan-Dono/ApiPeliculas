using ApiPeliculas.DTOs;
using ApiPeliculas.Entities;
using AutoMapper;

namespace ApiPeliculas.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap(); // Desde/Hacia y reversa
            CreateMap<GeneroCreacionDTO, Genero>();

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCrecionDTO, Actor>()
                .ForMember(x => x.Foto, opciones => opciones.Ignore());
        }
    }
}

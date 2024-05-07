using System.ComponentModel.DataAnnotations;
using ApiPeliculas.Helpers;
using ApiPeliculas.Validations;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.DTOs
{
    public class PeliculaCreacionDTO
    {
        [Required]
        [StringLength(300)]
        public string Titulo { get; set; }
        public bool EnCines { get; set; }
        public DateTime FechaEstreno { get; set; }
        [PesoArchivoValidacion(pesoMaximoEnMb: 4)]
        [TipoArchivoValidacion(GrupoTipoArchivos.Image)]
        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenerosIDs { get; set; }

        [ModelBinder(BinderType = typeof (TypeBinder<List<ActorPeliculaCreacionDTO>>))]
        public List<ActorPeliculaCreacionDTO> Actores { get; set; }
    }
}

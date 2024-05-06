using System.ComponentModel.DataAnnotations;
using ApiPeliculas.Validations;

namespace ApiPeliculas.DTOs
{
    public class PeliculaCreacionDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Titulo { get; set; }
        public bool EnCines { get; set; }
        public DateTime FechaEstreno { get; set; }
        [PesoArchivoValidacion(pesoMaximoEnMb: 4)]
        [TipoArchivoValidacion(GrupoTipoArchivos.Image)]
        public IFormFile Poster { get; set; }

        public List<int> GenerosIDs { get; set; }
    }
}

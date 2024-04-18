using System.ComponentModel.DataAnnotations;
using ApiPeliculas.Validations;

namespace ApiPeliculas.DTOs
{
    public class ActorCrecionDTO
    {
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        [PesoArchivoValidacion(pesoMaximoEnMb:4)]
        [TipoArchivoValidacion(grupoTipoArchivos:GrupoTipoArchivos.Image)]
        public IFormFile Foto { get; set; } 

    }
}

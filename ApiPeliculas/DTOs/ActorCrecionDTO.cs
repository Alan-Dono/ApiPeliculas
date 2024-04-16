using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.DTOs
{
    public class ActorCrecionDTO
    {
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }

    }
}

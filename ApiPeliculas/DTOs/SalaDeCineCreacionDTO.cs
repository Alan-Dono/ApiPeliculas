using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.DTOs
{
    public class SalaDeCineCreacionDTO
    {
        [Required]
        [StringLength(80)]
        public string Nombre { get; set; }
    }
}

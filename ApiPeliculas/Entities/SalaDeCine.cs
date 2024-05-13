using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Entities
{
    public class SalaDeCine : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(80)]
        public string Nombre { get; set; }
        public List<PeliculasSalasDeCine> PeliculasSalasDeCines { get; set; }
    }
}

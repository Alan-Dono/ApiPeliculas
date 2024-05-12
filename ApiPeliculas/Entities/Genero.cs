using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Entities
{
    public class Genero : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public string Nombre { get; set; }
        public List<PeliculaGenero> PeliculasGeneros { get; set; }

    }
}

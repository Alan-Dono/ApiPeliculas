namespace ApiPeliculas.DTOs
{
    public class PeliculaDetallerDTO : PeliculaDTO
    {
        public List<GeneroDTO> Generos { get; set; }
        public List<ActorPeliculaDetalleDTO> Actores   { get; set; }
    }
}

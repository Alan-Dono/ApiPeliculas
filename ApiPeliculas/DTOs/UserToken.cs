using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.DTOs
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }
}

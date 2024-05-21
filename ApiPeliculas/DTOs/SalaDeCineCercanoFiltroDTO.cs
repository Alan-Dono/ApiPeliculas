using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.DTOs
{
    public class SalaDeCineCercanoFiltroDTO
    {
        public double Latitud { get; set; }
        [Range(-180, 180)]
        public double Longitud { get; set; }
        private int distanciaEnKms = 10;
        private int distanciaMaximaEnKms = 100;
        public int DistanciaEnKms
        {
            get { return distanciaEnKms; }
            set
            {
                distanciaEnKms = (value > distanciaMaximaEnKms) ? distanciaMaximaEnKms : value;
            }
        }
    }
}

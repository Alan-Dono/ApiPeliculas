using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Validations
{
    public class PesoArchivoValidacion : ValidationAttribute
    {
        private readonly int PesoMaximoEnMb;

        public PesoArchivoValidacion(int pesoMaximoEnMb)
        {
            PesoMaximoEnMb = pesoMaximoEnMb;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) 
            {
                return ValidationResult.Success; 
            }
            IFormFile formFile = value as IFormFile;
            if (formFile == null)
            {
                return ValidationResult.Success;
            }
            if (formFile.Length > PesoMaximoEnMb * 1024 * 1024)
            {
                return new ValidationResult($"El tamaño del archivo no puede ser mayor a {PesoMaximoEnMb} mb");
            }
            return ValidationResult.Success;
        }

    }
}

using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Validations
{
    public class TipoArchivoValidacion : ValidationAttribute
    {
        private readonly string[] TiposValidos;

        public TipoArchivoValidacion(string[] tiposValidos)
        {
            TiposValidos = tiposValidos;
        }

        public TipoArchivoValidacion(GrupoTipoArchivos grupoTipoArchivos)
        {
            if (grupoTipoArchivos == GrupoTipoArchivos.Image)
            {
                TiposValidos = new string[] { "image/jpeg", "image/png", "image/gif" };
            }
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
            if (!TiposValidos.Contains(formFile.ContentType))
            {
                return new ValidationResult($"Solo se admiten archivos tipo {string.Join(": ", TiposValidos)}");
            }
            return ValidationResult.Success;
        }

    }
}

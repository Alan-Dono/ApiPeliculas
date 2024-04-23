namespace ApiPeliculas.Services
{
    public interface IAlmacenadorArchivos
    {
        public Task<string> GuardarArchivo(byte[] contenido, string extension, string contenedor, string contentType);
        public Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor, string contentType, string ruta);
        public Task BorrarArchivo(string ruta, string contenedor);
    }
}

namespace ApiPeliculas.Services
{
    public class AlmacenadorArchivosLocal : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AlmacenadorArchivosLocal(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }
        public Task BorrarArchivo(string ruta, string contenedor)
        {
            if( ruta != null)
            {
                var nombreArchivo = Path.GetFileName(ruta);
                string directorioAarchivo = Path.Combine(env.WebRootPath, contenedor,nombreArchivo);

                if (File.Exists(directorioAarchivo))
                {
                    File.Delete(directorioAarchivo);
                }
            }
            return Task.CompletedTask;
        }

        public async Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor, string contentType, string ruta)
        {
            await BorrarArchivo(ruta, contenedor);
            return await GuardarArchivo(contenido, extension,contenedor,contentType);
        }

        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string contenedor, string contentType)
        {
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(env.WebRootPath, contenedor);

            if(!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);  
            }
            string ruta = Path.Combine(folder,nombreArchivo);
            await File.WriteAllBytesAsync(ruta, contenido);

            var urlActual = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var urlParaBD = Path.Combine(urlActual,contenedor, nombreArchivo).Replace("\\","/");
            return urlParaBD;
        }
    }
}

using ApiPeliculas.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(opciones => 
                opciones.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")) // Configuramos el servicio de EF
            );

            services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosAzure>(); // servicio para almacenar archivos en azure

            services.AddAutoMapper(typeof(Startup)); // Indicamos que en este proyecto va a estar las clase la cual va a encapsular las configuraciones de automapper

            services.AddControllers();

            services.AddEndpointsApiExplorer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}


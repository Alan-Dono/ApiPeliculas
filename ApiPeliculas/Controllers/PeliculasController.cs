using ApiPeliculas.DTOs;
using ApiPeliculas.Entities;
using ApiPeliculas.Services;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "peliculas";

        public PeliculasController(ApplicationDbContext context,
            IMapper mapper,
            IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }


        [HttpGet]
        public async Task<ActionResult<List<PeliculaDTO>>> Get()
        {
            var peliculas = await context.Peliculas.ToListAsync();
            return mapper.Map<List<PeliculaDTO>>(peliculas);
        }

        [HttpGet("{id}", Name ="obtenerPelicula")]
        public async Task<ActionResult<PeliculaDTO>>Get(int id)
        {
            var pelicula = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id); 
            if (pelicula == null)
            {
                return NotFound();
            }
            return mapper.Map<PeliculaDTO>(pelicula);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);

          //  return NoContent(); // prueba
            if (peliculaCreacionDTO.Poster != null)
            {
                using (var memoryString = new MemoryStream()) // Esta memoria existe solo en el bloque using
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryString);
                    var contenido = memoryString.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    pelicula.Poster = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor, peliculaCreacionDTO.Poster.ContentType);
                }
            }
            AsignarOrdenActores(pelicula);
            context.Add(pelicula);
            await context.SaveChangesAsync();
            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);
            return new CreatedAtRouteResult("obtenerPelicula", new { id = pelicula.Id }, peliculaDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var peliculaDB = await context.Peliculas
                .Include(x => x.PeliculasActores)
                .Include(x => x.PeliculasGeneros)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (peliculaDB == null) { return NotFound(); }
            peliculaDB = mapper.Map(peliculaCreacionDTO, peliculaDB);
            if (peliculaCreacionDTO.Poster != null)
            {
                using (var memoryString = new MemoryStream()) // Esta memoria existe solo en el bloque using
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryString);
                    var contenido = memoryString.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    peliculaDB.Poster = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor, peliculaCreacionDTO.Poster.ContentType, peliculaDB.Poster);
                }
            }
            AsignarOrdenActores(peliculaDB);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<PeliculaPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            var entidadDB = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);
            if (entidadDB == null)
            {
                return NotFound();
            }
            var peliculaDTO = mapper.Map<PeliculaPatchDTO>(entidadDB);
            patchDocument.ApplyTo(peliculaDTO, ModelState);
            var esValido = TryValidateModel(peliculaDTO);
            if (!esValido)
            {
                return BadRequest(ModelState);
            }
            mapper.Map(peliculaDTO, entidadDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Peliculas.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Pelicula() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }

        private void AsignarOrdenActores(Pelicula pelicula)
        {
            if (pelicula.PeliculasActores != null)
            {
                for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
                {
                    pelicula.PeliculasActores[i].Orden = i;
                }
            }
        }


    }
    
}

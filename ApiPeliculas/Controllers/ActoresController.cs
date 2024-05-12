using ApiPeliculas.DTOs;
using ApiPeliculas.Entities;
using ApiPeliculas.Helpers;
using ApiPeliculas.Services;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController : CustomBaseController 
    {
        private readonly ApplicationDbContext _Context;
        private readonly IMapper _Mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "actores";
        public ActoresController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos):base(context, mapper)
        {
            _Context = context;
            _Mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            return await Get<Actor, ActorDTO>(paginacionDTO);
        }


        [HttpGet("{id:int}", Name = "GetActorById")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            return await Get<Actor, ActorDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCrecionDTO actorCrecionDTO)
        {
            var actor = _Mapper.Map<Actor>(actorCrecionDTO);
            if(actorCrecionDTO.Foto != null)
            {
                using(var memoryString = new MemoryStream()) // Esta memoria existe solo en el bloque using
                {
                    await actorCrecionDTO.Foto.CopyToAsync(memoryString);
                    var contenido = memoryString.ToArray();
                    var extension = Path.GetExtension(actorCrecionDTO.Foto.FileName);
                    actor.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor, actorCrecionDTO.Foto.ContentType);
                }
            }
            _Context.Add(actor);
            await _Context.SaveChangesAsync();
            var actorDto = _Mapper.Map<ActorDTO>(actor);
            return new CreatedAtRouteResult("GetActorById", new { id = actor.Id }, actorDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCrecionDTO actorCrecionDTO)
        {
            /*var actor = _Mapper.Map<Actor>(actorCrecionDTO);  // primera tecnica
            actor.Id = id;
            _Context.Entry(actor).State = EntityState.Modified;*/
            var actorDb = await _Context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if (actorDb == null) { return NotFound(); }
            actorDb = _Mapper.Map(actorCrecionDTO, actorDb);
            if (actorCrecionDTO.Foto != null)
            {
                using (var memoryString = new MemoryStream()) // Esta memoria existe solo en el bloque using
                {
                    await actorCrecionDTO.Foto.CopyToAsync(memoryString);
                    var contenido = memoryString.ToArray();
                    var extension = Path.GetExtension(actorCrecionDTO.Foto.FileName);
                    actorDb.Foto = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor, actorCrecionDTO.Foto.ContentType,actorDb.Foto);
                }
            }
            await _Context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ActorPatchDTO> patchDocument)
        {
            return await Patch<Actor, ActorPatchDTO>(id, patchDocument);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Actor>(id);
        }
    }
}

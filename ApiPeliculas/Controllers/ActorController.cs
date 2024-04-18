using ApiPeliculas.DTOs;
using ApiPeliculas.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActorController : ControllerBase 
    {
        private readonly ApplicationDbContext _Context;
        private readonly IMapper _Mapper;

        public ActorController(ApplicationDbContext context, IMapper mapper)
        {
            _Context = context;
            _Mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get()
        {
            var actores = await _Context.Actores.ToListAsync();
            var dtos = _Mapper.Map<List<ActorDTO>>(actores);
            return dtos;
        }

        [HttpGet("{id:int}", Name = "GetActorById")]
        public async Task<ActionResult<ActorDTO>> GetById(int id)
        {
            var actor = await _Context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            return _Mapper.Map<ActorDTO>(actor);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCrecionDTO actorCrecionDTO)
        {
            var actor = _Mapper.Map<Actor>(actorCrecionDTO);
            _Context.Add(actor);
            //await _Context.SaveChangesAsync();
            var actorDto = _Mapper.Map<ActorDTO>(actor);
            return new CreatedAtRouteResult("GetActorById", new { id = actor.Id }, actorDto);
        }

        [HttpPut("id:int")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCrecionDTO actorCrecionDTO)
        {
            var actor = _Mapper.Map<Actor>(actorCrecionDTO);
            actor.Id = id;
            _Context.Entry(actor).State = EntityState.Modified;
            await _Context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await _Context.Actores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            _Context.Remove(new Actor() { Id = id });
            await _Context.SaveChangesAsync();
            return NoContent();
        }
    }
}

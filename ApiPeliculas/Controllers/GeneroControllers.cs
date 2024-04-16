using ApiPeliculas.DTOs;
using ApiPeliculas.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GeneroControllers : ControllerBase
    {
        #region Properties
        private readonly ApplicationDbContext _Context;
        private readonly IMapper _Mapper;
        #endregion

        #region Builder
        public GeneroControllers(ApplicationDbContext context, IMapper mapper)
        {
            _Context = context;
            _Mapper = mapper;
        }

        #endregion

        #region Endpoints
        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get()
        {
            var entidades = await _Context.Generos.ToListAsync();
            var dtos = _Mapper.Map<List<GeneroDTO>>(entidades);
            return dtos;
        }

        [HttpGet("id:int", Name = "GetGeneroById")]
        public async Task<ActionResult<GeneroDTO>> GetById(int id)
        {
            var genero = await _Context.Generos.FirstOrDefaultAsync(x => x.Id == id);
            if (genero == null)
            {
                return NotFound();
            }
            var generoDTO = _Mapper.Map<GeneroDTO>(genero);
            return generoDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO creacionDTO)
        {
            var genero = _Mapper.Map<Genero>(creacionDTO);
            _Context.Add(genero);
            await _Context.SaveChangesAsync();
            var generoDTO = _Mapper.Map<GeneroDTO>(genero);
            return new CreatedAtRouteResult("GetGeneroById", new { id = generoDTO.Id }, generoDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = _Mapper.Map<Genero>(generoCreacionDTO);
            genero.Id = id;
            _Context.Entry(genero).State = EntityState.Modified;
            await _Context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await _Context.Generos.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            _Context.Remove(new Genero() { Id = id });
            await _Context.SaveChangesAsync();
            return NoContent();
        }
        #endregion
    }
}

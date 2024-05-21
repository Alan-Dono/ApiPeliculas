using System.Reflection;
using ApiPeliculas.DTOs;
using ApiPeliculas.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GenerosControllers : CustomBaseController
    {
        #region Properties
        //private readonly ApplicationDbContext _Context;
        //private readonly IMapper _Mapper;
        #endregion

        #region Builder
        public GenerosControllers(ApplicationDbContext context, IMapper mapper):base(context,mapper)
        {
           // _Context = DbContext;
           //_Mapper = mapper;
        }

        #endregion

        #region Endpoints
        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get()
        {
            return await Get<Genero, GeneroDTO>();
        }

        [HttpGet("id:int", Name = "GetGeneroById")]
        public async Task<ActionResult<GeneroDTO>> GetById(int id)
        {
            return await Get<Genero,GeneroDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            return await Post<GeneroCreacionDTO, Genero, GeneroDTO>(generoCreacionDTO, "GetGeneroById");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            return await Put<GeneroCreacionDTO, Genero>(id, generoCreacionDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Genero>(id);
        }
        #endregion
    }
}

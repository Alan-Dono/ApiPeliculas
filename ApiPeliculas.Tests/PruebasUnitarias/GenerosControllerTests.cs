using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiPeliculas.Controllers;
using ApiPeliculas.DTOs;
using ApiPeliculas.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiPeliculas.Tests.PruebasUnitarias
{
    [TestClass]
    public class GenerosControllerTests : BasePruebas
    {
        [TestMethod]
        public async Task ObtenerGeneros()
        {
            // Preparacion
            var nombreDb = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDb);
            var mapper = ConfigurarAutoMapper();
            contexto.Generos.Add(new Genero { Nombre = "Genero 1" });
            contexto.Generos.Add(new Genero { Nombre = "Genero 2" });
            await contexto.SaveChangesAsync();
            var contexto2 = ConstruirContext(nombreDb);


            // Prueba
            var controller = new GenerosControllers(contexto2,mapper);
            var respuesta = await controller.Get();

            // Verificacion
            var generos = respuesta.Value;
            Assert.AreEqual(2, generos.Count);
        }

        [TestMethod]
        public async Task ObtenerGeneroPorIdInexistente()
        {
            // Preparacion
            var nombreDb = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDb);
            var mapper = ConfigurarAutoMapper();

            // Prueba
            var controller = new GenerosControllers(contexto, mapper);
            var respuesta = await controller.GetById(1);

            // Verificacion
            var resultado = respuesta.Result as StatusCodeResult;
            Assert.AreEqual(404, resultado.StatusCode);
        }

        [TestMethod]
        public async Task ObtenerGeneroPorId()
        {
            var nombreDb = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDb);
            var mapper = ConfigurarAutoMapper();
            contexto.Generos.Add(new Genero { Nombre = "Genero 1" });
            contexto.Generos.Add(new Genero { Nombre = "Genero 2" });
            await contexto.SaveChangesAsync();

            var contxto2 = ConstruirContext(nombreDb);
            var controller = new GenerosControllers(contxto2, mapper);

            //var id = 1;
            var respuesta = await controller.GetById(1);
            var resultado = respuesta.Value;
            Assert.AreEqual(1, resultado.Id);

        }

        [TestMethod]
        public async Task CrearGenero()
        {
            var nombreDb = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDb);
            var mapper = ConfigurarAutoMapper();

            var nuevoGenero = new GeneroCreacionDTO(){ Nombre = "nuevo genero"};
            var controller = new GenerosControllers(contexto,mapper);
            var respuesta = await controller.Post(nuevoGenero);
            var resultado = respuesta as CreatedAtRouteResult;
            Assert.IsNotNull(resultado);
            
            var contexto2 = ConstruirContext(nombreDb);
            var cantidad = await contexto2.Generos.CountAsync();
            Assert.AreEqual(1, cantidad);

        }

        [TestMethod]
        public async Task ActualizarGener()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            contexto.Generos.Add(new Genero() { Nombre = "Género 1" });
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreBD);
            var controller = new GenerosControllers(contexto2, mapper);

            var generoCreacionDTO = new GeneroCreacionDTO() { Nombre = "Nuevo nombre" };

            var id = 1;
            var respuesta = await controller.Put(id, generoCreacionDTO);

            var resultado = respuesta as StatusCodeResult;
            Assert.AreEqual(204, resultado.StatusCode);

            var contexto3 = ConstruirContext(nombreBD);
            var existe = await contexto3.Generos.AnyAsync(x => x.Nombre == "Nuevo nombre");
            Assert.IsTrue(existe);
        }

        [TestMethod]
        public async Task IntentaBorrarGeneroNoExistente()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            var controller = new GenerosControllers(contexto, mapper);

            var respuesta = await controller.Delete(1);
            var resultado = respuesta as StatusCodeResult;
            Assert.AreEqual(404, resultado.StatusCode);
        }

        [TestMethod]
        public async Task BorrarGenero()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            contexto.Generos.Add(new Genero() { Nombre = "Género 1" });
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreBD);
            var controller = new GenerosControllers(contexto2, mapper);

            var respuesta = await controller.Delete(1);
            var resultado = respuesta as StatusCodeResult;
            Assert.AreEqual(204, resultado.StatusCode);

            var contexto3 = ConstruirContext(nombreBD);
            var existe = await contexto3.Generos.AnyAsync();
            Assert.IsFalse(existe);
        }

    }
}

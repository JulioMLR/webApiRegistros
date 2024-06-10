﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using webApiRegistros.DTOs;
using webApiRegistros.Entidades;

namespace webApiRegistros.Controllers
{

    [ApiController]
    [Route("Objeto")]

    public class ObjetoController: ControllerBase
    {

        private readonly ApplicationDbContext dbContext;

        public ObjetoController(ApplicationDbContext context)
        {

            this.dbContext = context;

        }

        [HttpPost("All")]

        //CREATE OBJETO
        public async Task<ActionResult> Post(ObjetoDTO objetoDTO)
        {

            var objeto = new Objeto()
            {

                nombre = objetoDTO.nombre,
                cantidad = objetoDTO.cantidad

            };

            dbContext.Add(objeto);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpGet("All")]

        //READ ALL OBJETO
        public async Task<ActionResult<List<Objeto>>> Get() {

            var listaObjetos = await dbContext.Objetos.ToListAsync();

            if (listaObjetos.IsNullOrEmpty())
            {

                return NotFound("No se encontraron objetos");

            }
            
            return listaObjetos;
        
        }

        [HttpGet("Id")]

        //READ ONLY ID OBJETO
        public async Task<ActionResult<Objeto>> GetId(int id)
        {

            var objeto = await dbContext.Objetos.Where(c => c.id == id).ToListAsync();

            if (objeto.IsNullOrEmpty())
            {

                return BadRequest("No se encontraron objetos con el Id");
            
            }

            return Ok(objeto);

        }

        [HttpGet("Cantidad")]

        //READ ONLY CANTIDAD
        public async Task<ActionResult<List<Objeto>>> GetCantidad(int cantidad)
        {

            var objetos = await dbContext.Objetos.Where(c => c.cantidad == cantidad).ToListAsync();

            if (objetos == null)
            {

                return NotFound("No se encontraron objetos con esa cantidad");

            }

            return Ok(objetos);

        }

        [HttpPut("All")]

        //UPDATE ALL OBJETO
        public async Task<ActionResult> Put(int id, Objeto objeto)
        {

            if(objeto.id != id)
            {

                return BadRequest("El id no coincide con el establecido");

            }

            var existeObjeto = await dbContext.Objetos.AnyAsync(c => c.id == id);

            if (!existeObjeto)
            {

                return BadRequest("No existe el objeto");

            }

            dbContext.Update(objeto);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpPut("Cantidad")]

        //UPDATE CANTIDAD IN OBJETO
        public async Task<ActionResult> Put(int id, int cantidad)
        {

            var existeObjeto = await dbContext.Objetos.AnyAsync(c => c.id == id);

            if (!existeObjeto)
            {

                return BadRequest("No existe el objeto");

            }

            Objeto objeto = await dbContext.Objetos.Where(c => c.id == id).FirstAsync();

            objeto.cantidad = cantidad;

            dbContext.Update(objeto);
            await dbContext.SaveChangesAsync();
            return Ok();

        }
        
        [HttpDelete("Objeto")]
        
        //DELETE OBJETO
        public async Task<ActionResult> Delete(int id)
        {

            var objeto = await dbContext.Objetos.Where(c => c.id == id).FirstAsync();

            if (objeto == null)
            {

                return BadRequest("No existe el objeto");

            }

            dbContext.Remove(objeto);
            dbContext.SaveChanges();
            return Ok();

        }

    }
}

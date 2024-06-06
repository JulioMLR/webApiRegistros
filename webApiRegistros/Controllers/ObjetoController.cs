using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        [HttpGet("All")]

        public async Task<ActionResult<List<Objeto>>> Get() {

            var objetos = await dbContext.Objetos.ToListAsync();

            if (objetos.IsNullOrEmpty())
            {

                return NotFound("No se encuentran objetos");

            }

            return objetos;
        
        }

        [HttpGet("Id")]

        public async Task<ActionResult<Objeto>> GetId(int id)
        {

            var objeto = await dbContext.Objetos.Where(c => c.id == id).ToListAsync();

            if (objeto.IsNullOrEmpty())
            {

                return BadRequest("No se encontraron objetos con el Id");
            
            }

            return Ok(objeto);

        }

        [HttpPost]

        public async Task<ActionResult> Post(Objeto objeto)
        {

            dbContext.Add(objeto);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpPut]

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

        [HttpDelete]

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

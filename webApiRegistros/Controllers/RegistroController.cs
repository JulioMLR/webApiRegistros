using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using webApiRegistros.Entidades;

namespace webApiRegistros.Controllers
{
    [ApiController]
    [Route("Registro")]

    public class RegistroController : ControllerBase
    {

        private readonly ApplicationDbContext dbContext;

        public RegistroController(ApplicationDbContext context)
        {

            this.dbContext = context;

        }

        [HttpPost]

        //CREATE REGISTRO
        public async Task<ActionResult> Post(Registro registro)
        {

            var existeObjeto = await dbContext.Objetos.AnyAsync(c => c.id == registro.idObjeto);

            if (!existeObjeto)
            {

                return BadRequest("No existe el objeto");

            }

            dbContext.Add(registro);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpPost(":id")]

        public async Task<ActionResult> Post(int idRegistro, int idObjeto)
        {

            var existeRegistro = await dbContext.Registros.AnyAsync(c => c.id == idRegistro);

            if (!existeRegistro)
            {

                return BadRequest("No existe el registro");

            }

            var existeObjeto = await dbContext.Objetos.AnyAsync(c => c.id == idObjeto);

            if (!existeObjeto)
            {

                return BadRequest("No existe el objeto");

            }

            var registro = await dbContext.Registros.Where(c => c.id == idRegistro).FirstAsync();

            registro.idObjeto = idObjeto;

            dbContext.Update(registro);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpGet("All")]

        //READ ALL
        public async Task<ActionResult<List<Registro>>> Get()
        {

            var listaRegistros = await dbContext.Registros.Include(c => c.objeto).ToListAsync();

            if (listaRegistros.IsNullOrEmpty())
            {

                return NotFound("No se encontraron registros");

            }

            return Ok(listaRegistros);

        }


        [HttpGet("Id")]

        //READ ONLY ID
        public async Task<ActionResult<Registro>> GetId(int id)
        {

            var listaRegistros = await dbContext.Registros.Where(c => c.id == id).Include(c => c.objeto).ToListAsync();

            if (listaRegistros.IsNullOrEmpty())
            {

                return BadRequest("No se encontro el registro");

            }

            return Ok(listaRegistros);

        }



        [HttpGet("Campo")]

        //READ ONLY CAMPO
        public async Task<ActionResult<List<Registro>>> GetCampo(string campo, string busqueda)
        {

            IQueryable<Registro> filtroRegistros;
            List<Registro> listaRegistros;

            switch (campo)
            {

                case "o1":

                    filtroRegistros = dbContext.Registros.Where(c => c.o1 == busqueda);

                    break;

                case "o2":

                    filtroRegistros = dbContext.Registros.Where(c => c.o2 == busqueda);

                    break;

                case "o3":

                    filtroRegistros = dbContext.Registros.Where(c => c.o3 == busqueda);

                    break;

                default:

                    return BadRequest("No existe el campo");

            }

            listaRegistros = await filtroRegistros.Include(c => c.objeto).ToListAsync();

            if (listaRegistros.IsNullOrEmpty())
            {

                return NotFound("No se encontraron registros");

            }

            return Ok(listaRegistros);

        }

        [HttpGet("Objeto")]

        public async Task<ActionResult<List<Objeto>>> GetObjetos(int idObjeto)
        {

            var listaRegistros = await dbContext.Registros.Where(c => c.idObjeto == idObjeto).ToListAsync();

            if (listaRegistros.IsNullOrEmpty())
            {

                return NotFound("No se encontraron registros");

            }

            return Ok(listaRegistros);

        }

        //UPDATE
        [HttpPut]
        public async Task<ActionResult> Put(Registro registro, int id)
        {

            if (registro.id != id)
            {

                return BadRequest("El Id no coincide con el establecido");

            }

            var existeRegistro = await dbContext.Registros.AnyAsync(c => c.id == registro.id);

            if (!existeRegistro)
            {

                return BadRequest("No existe el registro con el Id");

            }

            var existeObjeto = await dbContext.Objetos.AnyAsync(c => c.id == registro.id);

            if (!existeObjeto)
            {

                return BadRequest("No existe el objeto con el Id");

            }

            dbContext.Update(registro);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

        //DELETE
        [HttpDelete]

        public async Task<ActionResult> Delete(int id)
        {

            var existeRegistro = await dbContext.Registros.AnyAsync(c => c.id == id);

            if (!existeRegistro)
            {

                return NotFound("No se encontro el registro");

            }

            dbContext.Registros.Remove(new Registro()
            {

                id = id

            });

            await dbContext.SaveChangesAsync();
            return Ok();

        }

    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using webApiRegistros.DTOs;
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

        [HttpPost("Registro")]

        //CREATE REGISTRO
        public async Task<ActionResult> Post(RegistroDTO registroDTO)
        {

            var registro = new Registro()
            {

                o1 = registroDTO.o1,
                o2 = registroDTO.o2,
                o3 = registroDTO.o3

            };

            dbContext.Add(registro);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpPost("Objeto")]

        //CREATE OBJETO IN REGISTRO
        public async Task<ActionResult> PostObjeto(int idRegistro, int idObjeto)
        {

            var registro = await dbContext.Registros.Include(c => c.objeto).FirstOrDefaultAsync(c => c.id == idRegistro);

            if (registro == null)
            {

                return NotFound("No existe el registro");

            }

            var objeto = await dbContext.Objetos.FirstOrDefaultAsync(c => c.id == idObjeto);

            if (objeto == null)
            {

                return NotFound("No existe el objeto");

            }

            registro.objeto.Add(objeto);

            dbContext.Update(registro);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpGet("All")]

        //READ ALL REGISTRO
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

        //READ ONLY ID REGISTRO
        public async Task<ActionResult<Registro>> GetId(int id)
        {

            var listaRegistros = await dbContext.Registros.Include(c => c.objeto).FirstOrDefaultAsync(c => c.id == id);

            if (listaRegistros == null)
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

        //READ ONLY ID OBJETO
        public async Task<ActionResult<List<Objeto>>> GetObjetos(int idObjeto)
        {

            List<Registro> listaRegistros = new List<Registro>();

            var registros = await dbContext.Registros.Include(c => c.objeto).ToListAsync();

            foreach(var registro in registros)
            {

                if(registro.objeto.Find(c => c.id == idObjeto) != null)
                {

                    listaRegistros.Add(registro);

                }

            }

            return Ok(listaRegistros);

        }

        [HttpPut("All")]

        //UPDATE REGISTRO
        public async Task<ActionResult> Put(RegistroIdDTO registroDTO, int id)
        {

            if (registroDTO.id != id)
            {

                return BadRequest("El Id no coincide con el establecido");

            }

            var registro = await dbContext.Registros.FirstOrDefaultAsync(c => c.id == registroDTO.id);

            if (registro == null)
            {

                return BadRequest("No existe el registro con el Id");

            }

            registro.o1 = registroDTO.o1;
            registro.o2 = registroDTO.o2;
            registro.o3 = registroDTO.o3;

            dbContext.Update(registro);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpPut("Campo")]

        //UPDATE CAMPO IN REGISTRO
        public async Task<ActionResult> PutCampo(string cambio, int idRegistro, string campo)
        {

            var registro = await dbContext.Registros.FirstOrDefaultAsync(c => c.id == idRegistro);

            if (registro == null)
            {

                return BadRequest("No existe el registro con el Id");

            }

            switch (campo)
            {

                case "o1":

                    registro.o1 = cambio;

                    break;

                case "o2":

                    registro.o2 = cambio;

                    break;

                case "o3":

                    registro.o3 = cambio;

                    break;

                default:

                    return BadRequest("No existe el campo");

            }

            dbContext.Update(registro);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpDelete("Registro")]

        //DELETE REGISTRO
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

        [HttpDelete("Objeto")]

        //DELETE OBJETO IN REGISTRO
        public async Task<ActionResult> DeleteObjeto(int idRegistro, int idObjeto)
        {

            var registro = await dbContext.Registros.Include(c => c.objeto).FirstOrDefaultAsync(c => c.id == idRegistro);

            if (registro == null)
            {

                return NotFound("No existe el registro");

            }

            var objeto = await dbContext.Objetos.FirstOrDefaultAsync(c => c.id == idObjeto);

            if (objeto == null)
            {

                return NotFound("No existe el objeto");

            }

            registro.objeto.Remove(objeto);

            dbContext.Update(registro);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

    }
}
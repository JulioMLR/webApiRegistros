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

        [HttpPost]

        //CREATE
        public async Task<ActionResult> Post(RegistroDTO registroDTO)
        {

            Registro registro = new Registro();

            var listaObjetos = await dbContext.Objetos.Where(c => c.id == registroDTO.idObjeto).ToListAsync();

            if (listaObjetos.IsNullOrEmpty())
            {

                return BadRequest("No existe el Objeto");

            }

            registro.o1 = registroDTO.o1;
            registro.o2 = registroDTO.o2;
            registro.o3 = registroDTO.o3;
            registro.objeto = await dbContext.Objetos.Where(c => c.id == registroDTO.idObjeto).FirstAsync();

            dbContext.Add(registro);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpGet("All")]

        //READ ALL
        public async Task<ActionResult<List<Registro>>> Get()
        {

            List<Registro> listaRegistros = await dbContext.Registros.ToListAsync();

            if (listaRegistros.IsNullOrEmpty())
            {

                return BadRequest("No se encontraron registros");

            }

            return Ok(listaRegistros);

        }


        [HttpGet("Id")]

        //READ ONLY ID
        public async Task<ActionResult<Registro>> GetId(int id)
        {

            var listaRegistros = await dbContext.Registros.Where(c => c.id == id).ToListAsync();

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

            List<Registro> listaRegistros;

            switch (campo)
            {

                case "o1":

                    listaRegistros = await dbContext.Registros.Where(c => c.o1 == busqueda).ToListAsync();

                    break;

                case "o2":

                    listaRegistros = await dbContext.Registros.Where(c => c.o2 == busqueda).ToListAsync();

                    break;

                case "o3":

                    listaRegistros = await dbContext.Registros.Where(c => c.o3 == busqueda).ToListAsync();

                    break;

                default:

                    return BadRequest("No existe el campo");

            }

            if (listaRegistros.IsNullOrEmpty())
            {

                return NotFound("No se encontro registros");

            }

            return Ok(listaRegistros);

        }

        [HttpGet("Objeto")]

        public async Task<ActionResult<List<Objeto>>> GetObjetos(int idObjeto)
        {

            List<Registro> registros = new List<Registro>();
            var listaRegistros = await dbContext.Registros.ToListAsync();

            foreach(Registro listaRegistro in listaRegistros)
            {

                if(listaRegistro.objeto.id == idObjeto)
                {

                    registros.Add(listaRegistro);

                }

            }

            if (registros.IsNullOrEmpty())
            {

                return NotFound("No se encontraron registros");

            }

            return Ok(registros);

        }

        //UPDATE
        [HttpPut]

        public async Task<ActionResult> Put(Registro registro, int id)
        {

            if (registro.id != id)
            {

                return BadRequest("El id no coincide con el establecido");

            }

            var existeRegistro = await dbContext.Registros.AnyAsync(c => c.id == registro.id);

            if (!existeRegistro)
            {

                return BadRequest("No existe el registro con el Id");

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
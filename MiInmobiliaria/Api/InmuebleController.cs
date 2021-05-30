using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiInmobiliaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MiInmobiliaria.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class InmuebleController : ControllerBase
    {
        private readonly DataContext contexto;

        public InmuebleController(DataContext contexto)
        {
            this.contexto = contexto;
        }

        // GET: api/<InmuebleController>
        [HttpGet]
        public async Task<IActionResult> GetByUsuario()
        {
            return Ok(contexto.Inmueble
                .Where(x => x.Propietario.Persona.Email == User.Identity.Name));
        }



        // GET api/<InmuebleController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(contexto.Inmueble
                .Include(x => x.TipoInmueble)
                .Include(x => x.UsoInmueble)
                .Include(x => x.Propietario)
                .ThenInclude(x => x.Persona)
                .Where(x => x.Propietario.Persona.Email == User.Identity.Name && x.Id == id)
                .Single());

            /*
            return Ok(contexto.Inmueble
                .Include(x => x.TipoInmueble)
                .Include(x => x.UsoInmueble)
                .Include(x => x.Propietario)
                .ThenInclude(x => x.Persona)
                .Where(x => x.Propietario.Persona.Email == usuario && x.Id == id)
                .Select(x =>
                new {
                    id = x.Id,
                    direccion = x.Direccion,
                    usoInmueble = x.UsoInmueble.Nombre,
                    tipoInmueble = x.TipoInmueble.Nombre,
                    ambientes = x.Ambientes,
                    precio = x.Precio,
                    proppietario = x.Propietario,
                    x.Disponible,
                    avatar = x.Avatar
                })
                .Single());
            */

        }




        // POST: api/Contrato
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add")]
        public async Task<ActionResult<Inmueble>> PostInmueble([FromBody] Inmueble entidad)
        {
            // Asegurarse que se agregen un inmueble al propietario logueado
            entidad.PropietarioId = contexto.Propietario.Include(x => x.Persona).AsNoTracking().FirstOrDefault(x => x.Persona.Email == User.Identity.Name).Id;

            contexto.Inmueble.Add(entidad);
            await contexto.SaveChangesAsync();

            return CreatedAtAction("GetInmueble", new { id = entidad.Id }, entidad);
        }



        // PUT: api/Contrato/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> EditInmueble(int id)
        {
            // Asegurarse que se modifique un inmueble del propietario logueado
            var entidad = contexto.Inmueble
                .Where(x => x.Propietario.Persona.Email == User.Identity.Name && x.Id == id).Single();
            if (entidad == null)
            {
                return BadRequest();
            }

            entidad.Disponible = !entidad.Disponible;
            contexto.Entry(entidad).State = EntityState.Modified;

            try
            {
                await contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InmuebleExists(entidad.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(contexto.Inmueble
                .Include(x => x.TipoInmueble)
                .Include(x => x.UsoInmueble)
                .Include(x => x.Propietario)
                .ThenInclude(x => x.Persona)
                .Where(x => x.Propietario.Persona.Email == User.Identity.Name && x.Id == id)
                .Single());
        }



        // DELETE: api/Contrato/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteInmueble(int id)
        {
            // Asegurarse que se elimine un inmueble del propietario logueado
            int PropietarioId = (contexto.Propietario.AsNoTracking().FirstOrDefault(x => x.Persona.Email == User.Identity.Name)).Id;

            var inmueble = await contexto.Inmueble.FindAsync(id);
            if (inmueble == null || inmueble.PropietarioId != PropietarioId)
            {
                return NotFound();
            }

            contexto.Inmueble.Remove(inmueble);
            await contexto.SaveChangesAsync();

            return NoContent();
        }


        private bool InmuebleExists(int id)
        {
            return contexto.Inmueble.Any(e => e.Id == id);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiInmobiliaria.Models;

namespace MiInmobiliaria.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ContratoController : ControllerBase
    {
        private readonly DataContext contexto;

        public ContratoController(DataContext context)
        {
            contexto = context;
        }


        // GET: api/Contrato
        [HttpGet]
        public async Task<IActionResult> GetAlquileres()
        {
            return Ok(contexto.Contrato
                .Include(x => x.Inmueble)
                .Include(x => x.Inquilino)
                .ThenInclude(x => x.Persona)
                .Where(x => x.Inmueble.Propietario.Persona.Email == User.Identity.Name
                    && x.Hasta >= DateTime.Now));
        }



        // GET: api/Contrato/5
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetDetalleInquilino(int Id)
        {
            return Ok(contexto.Contrato
                .Include(x => x.Inmueble)
                .Include(x => x.Inquilino)
                .ThenInclude(x => x.Persona)
                .Where(x => x.Inmueble.Propietario.Persona.Email == User.Identity.Name && x.Id == Id).Single());
        }


        // PUT: api/Contrato/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContrato(int id, Contrato contrato)
        {
            if (id != contrato.Id)
            {
                return BadRequest();
            }

            contexto.Entry(contrato).State = EntityState.Modified;

            try
            {
                await contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContratoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Contrato
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Contrato>> PostContrato(Contrato contrato)
        {
            contexto.Contrato.Add(contrato);
            await contexto.SaveChangesAsync();

            return CreatedAtAction("GetContrato", new { id = contrato.Id }, contrato);
        }

        // DELETE: api/Contrato/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContrato(int id)
        {
            var contrato = await contexto.Contrato.FindAsync(id);
            if (contrato == null)
            {
                return NotFound();
            }

            contexto.Contrato.Remove(contrato);
            await contexto.SaveChangesAsync();

            return NoContent();
        }

        private bool ContratoExists(int id)
        {
            return contexto.Contrato.Any(e => e.Id == id);
        }
    }
}

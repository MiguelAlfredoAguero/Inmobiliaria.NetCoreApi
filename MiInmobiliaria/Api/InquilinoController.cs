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
    public class InquilinoController : ControllerBase
    {
        private readonly DataContext contexto;

        public InquilinoController(DataContext contexto)
        {
            this.contexto = contexto;
        }

        // GET: api/<InquilinoController>
        [HttpGet]
        public async Task<IActionResult> GetAllInquilinos()
        {
            return Ok(contexto.Contrato
                .Include(x => x.Inmueble)
                .Include(x => x.Inquilino)
                .ThenInclude(x => x.Persona)
                .Where(x => x.Inmueble.Propietario.Persona.Email == User.Identity.Name 
                    && x.Hasta >= DateTime.Now));
        }

        // GET api/<InquilinoController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllInquilinos(int id)
        {
            return Ok(contexto.Contrato
                .Include(x => x.Inmueble)
                .Include(x => x.Inquilino)
                .ThenInclude(x => x.Persona)
                .Include(x => x.Garante)
                .ThenInclude(x => x.Persona)
                .Where(x => x.Inmueble.Propietario.Persona.Email == User.Identity.Name && x.Id == id).Single());
        }

        // POST api/<InquilinoController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<InquilinoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InquilinoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiInmobiliaria.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MiInmobiliaria.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PropietarioController : ControllerBase
    {
        private readonly IRepositorioPropietario repositorio;
        private readonly DataContext contexto;
        private readonly IConfiguration configuration;
        private readonly Utils utils;

        public PropietarioController(DataContext contexto, IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.repositorio = new RepositorioPropietario(configuration);
            this.contexto = contexto;
            this.configuration = configuration;
            this.utils = new Utils(configuration, environment);
        }


        // GET api/<PropietarioController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(contexto.Propietario
                .Include(x => x.Persona)
                .ThenInclude(x => x.TipoPersona)
                .SingleOrDefault(x => x.Id == id));
        }



        // POST api/<controller>/login
        [HttpGet("propietarioActual")]
        public async Task<IActionResult> PropietarioActual()
        {
            try
            {
                var personaView = contexto.Persona
                    .Select(x => new { x.Id, x.Dni, x.Nombre, x.Apellido, x.Email, Contraseña = x.Password, x.Telefono, x.FechaNac })
                    .FirstOrDefault(x => x.Email == User.Identity.Name);

                return Ok(personaView);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        //POST api/<controller>/5
        [HttpPut("edit")]
        public async Task<IActionResult> EditPerfil([FromBody] PropietarioView entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Asegurarse que se editen los datos del propietario logueado
                    var propietario = contexto.Propietario
                        .Include(x => x.Persona)
                        .FirstOrDefault(x => x.Persona.Email == User.Identity.Name);

                    if ( !entidad.Contraseña.Equals("") )
                        propietario.Persona.Password = utils.getPasswordHashed(entidad.Contraseña);
                    else
                        propietario.Persona.Password = (contexto.Persona.AsNoTracking().FirstOrDefault(x => x.Email == User.Identity.Name)).Password;
                    propietario.Persona.Nombre = entidad.Nombre;
                    propietario.Persona.Apellido = entidad.Apellido;
                    propietario.Persona.Dni = entidad.Dni.ToString();
                    propietario.Persona.FechaNac = Convert.ToDateTime(entidad.FechaNac);
                    propietario.Persona.Email = entidad.Email;
                    contexto.Update(propietario);
                    contexto.SaveChanges();
                    return Ok(entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }




        // POST api/<controller>/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginView loginView)
        {
            try
            {
                string hashed = utils.getPasswordHashed(loginView.Password);
                var p = contexto.Propietario
                    .Include(x => x.Persona)
                    .FirstOrDefault(x => x.Persona.Email == loginView.User);
                if (p == null || p.Persona.Password != hashed)
                {
                    return BadRequest("Nombre de usuario o clave incorrecta");
                }
                else
                {
                    var key = new SymmetricSecurityKey(
                        System.Text.Encoding.ASCII.GetBytes(configuration["TokenAuthentication:SecretKey"]));
                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, p.Persona.Email),
                        new Claim("FullName", p.Persona.Nombre + " " + p.Persona.Apellido),
                        new Claim(ClaimTypes.Role, "Propietario"),
                    };

                    var token = new JwtSecurityToken(
                        issuer: configuration["TokenAuthentication:Issuer"],
                        audience: configuration["TokenAuthentication:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: credenciales
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


    }
}

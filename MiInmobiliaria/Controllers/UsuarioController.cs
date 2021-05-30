using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MiInmobiliaria.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiInmobiliaria.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly IRepositorioUsuario repositorio;
        private readonly IRepositorioPersona repositorioPersona;
        private readonly IRepositorioTipoPersona repositorioTipoPersona;
        private readonly Utils utils;

        public UsuarioController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.repositorio = new RepositorioUsuario(configuration);
            this.repositorioTipoPersona = new RepositorioTipoPersona(configuration);
            this.repositorioPersona = new RepositorioPersona(configuration);
            this.environment = environment;
            this.utils = new Utils(configuration, environment);
        }


        // GET: Usuarios/Login/
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }


        // POST: Usuarios/Login/
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Login login)
        {
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Password,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    var e = repositorio.getByEmail(login.User);
                    if (e == null || e.Persona.Password != hashed)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, e.Persona.Email),
                        new Claim("FullName", e.Persona.Nombre + " " + e.Persona.Apellido),
                        new Claim(ClaimTypes.Role, e.Persona.RolNombre),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));
                    TempData.Remove("returnUrl");
                    return Redirect(returnUrl);
                }
                TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }




        [Authorize(Policy = "Administrador")]
        // GET: UsuarioController
        public ActionResult Index()
        {
            ViewData["Error"] = TempData["Error"];
            ViewData["Warning"] = TempData["Warning"];
            ViewData["Success"] = TempData["Success"];

            var lista = repositorio.getAll();
            return View(lista);
        }


        [Authorize(Policy = "Administrador")]
        // GET: UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }


        [Authorize(Policy = "Administrador")]
        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.TiposPersona = repositorioTipoPersona.getAll();
                ViewBag.Roles = Persona.ObtenerRoles();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrio un error." + ex.ToString();
                return RedirectToAction(nameof(Index));
            }

        }

        [Authorize(Policy = "Administrador")]
        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario e)
        {
            try
            {
                Persona p = repositorioPersona.getByDniEmail(e.Persona.Dni, e.Persona.Email);
                if (p != null)
                    e.Persona = p;
                else
                {
                    e.Persona.TipoPersona = repositorioTipoPersona.getById(e.Persona.TipoPersona.Id);
                    e.Persona.TipoPersonaId = e.Persona.TipoPersona.Id;
                    e.Persona.Password = utils.getPasswordHashed(e.Persona.Password);
                    e.Persona.Rol = (User.IsInRole("Administrador") || User.IsInRole("SuperAdministrador")) ? e.Persona.Rol : (int)enRoles.Empleado;
                    e.Persona.Avatar = "";
                    e.Persona.Id = repositorioPersona.Create(e.Persona);
                    if (e.Persona.AvatarFile != null)
                    {
                        e.Persona.Avatar = utils.uploadFile(e.Persona);
                    }
                    repositorioPersona.Edit(e.Persona);
                }


                e.Activo = true;
                repositorio.Create(e);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrio un error." + ex.ToString();
                return RedirectToAction(nameof(Index));
            }
        }




        // GET: Usuarios/Edit/5
        [Authorize]
        public ActionResult Perfil()
        {
            ViewBag.TiposPersona = repositorioTipoPersona.getAll();
            ViewData["Title"] = "Mi perfil";
            var u = repositorio.getByEmail(User.Identity.Name);
            ViewBag.Roles = Persona.ObtenerRoles();
            return View("Edit", u);
        }


        [Authorize(Policy = "Administrador")]
        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.TiposPersona = repositorioTipoPersona.getAll();
            ViewBag.Roles = Persona.ObtenerRoles();
            var e = repositorio.getById(id);
            return View(e);
        }

        [Authorize]
        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Usuario e)
        {
            try
            {
                e.Persona.TipoPersona = repositorioTipoPersona.getById(e.Persona.TipoPersona.Id);
                e.Persona.TipoPersonaId = e.Persona.TipoPersona.Id;
                e.Persona.Password = utils.getPasswordHashed(e.Persona.Password);
                //Fuente: https://es.coredump.biz/questions/4538894/get-index-of-a-keyvalue-pair-in-a-c-dictionary-based-on-the-value
                //e.Persona.Rol = Persona.ObtenerRoles().First(kvp => kvp.Value.Equals("Empleado")).Key;
                e.Persona.Rol = (User.IsInRole("Administrador") || User.IsInRole("SuperAdministrador")) ? e.Persona.Rol : (int)enRoles.Empleado;
                e.Persona.Avatar = "";
                if (e.Persona.AvatarFile != null)
                {
                    e.Persona.Avatar = utils.uploadFile(e.Persona);
                }
                repositorioPersona.Edit(e.Persona);

                e.Activo = true;
                repositorio.Edit(e);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrio un error." + ex.ToString();
                return View();
            }
        }



        [Authorize(Policy = "Administrador")]
        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }

        [Authorize(Policy = "Administrador")]
        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Usuario e)
        {
            try
            {
                repositorio.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                TempData["Error"] = ex.Number == 547 ? "No se puede borrar el Usuario porque esta utilizado" : "Ocurrio un error.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrio un error." + ex.ToString();
                return RedirectToAction(nameof(Index));
            }
        }



        // GET: /salir
        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }

}

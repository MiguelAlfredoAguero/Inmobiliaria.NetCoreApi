using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MiInmobiliaria.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Controllers
{
    public class PersonaController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly IRepositorioPersona repositorio;
        private readonly IRepositorioTipoPersona repositorioTipoPersona;
        private readonly Utils utils;

        public PersonaController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = new RepositorioPersona(configuration);
            this.repositorioTipoPersona = new RepositorioTipoPersona(configuration);
            this.utils = new Utils(configuration, environment);
        }

        // GET: PersonaController
        public ActionResult Index()
        {
            ViewData["Error"] = TempData["Error"];
            ViewData["Warning"] = TempData["Warning"];
            ViewData["Success"] = TempData["Success"];

            var lista = repositorio.getAll();
            ViewData[nameof(Persona)] = lista;
            ViewData["Title"] = nameof(Persona);
            return View(lista);
        }

        // GET: PersonaController/Details/5
        public ActionResult Details(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }



        // GET: PersonaController/Create
        public ActionResult Create()
        {
            ViewBag.TiposPersona = repositorioTipoPersona.getAll();
            return View();
        }

        // POST: PersonaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Persona e)
        {
            try
            {
                Persona p = repositorio.getByDniEmail(e.Dni, e.Email);
                if (p != null)
                    e = p;
                else
                {
                    e.Password = "";
                    e.Avatar = "";
                    e.Id = repositorio.Create(e);
                    if (e.AvatarFile != null)
                    {
                        e.Avatar = utils.uploadFile(e);
                        repositorio.Edit(e);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrio un error." + ex.ToString();
                return View();
            }

        }



        // GET: PersonaController/Edit/5
        public ActionResult Edit(int id)
        {
            //ViewBag.items = repositorioTipoPersona.ListarSelectListItem();
            ViewBag.TiposPersona = repositorioTipoPersona.getAll();
            var e = repositorio.getById(id);
            return View(e);
        }

        // POST: PersonaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Persona e)
        {
            try
            {
                e.Password = repositorio.getById(e.Id).Password;
                e.Avatar = "";
                if (e.AvatarFile != null)
                {
                    e.Avatar = utils.uploadFile(e);
                }
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
        // GET: PersonaController/Delete/5
        public ActionResult Delete(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }
        [Authorize(Policy = "Administrador")]
        // POST: PersonaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Persona e)
        {
            try
            {
                repositorio.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                TempData["Error"] = ex.Number == 547 ? "No se puede borrar la Persona porque esta utilizado" : "Ocurrio un error.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrio un error." + ex.ToString();
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

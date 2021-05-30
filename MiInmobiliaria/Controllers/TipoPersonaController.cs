using Microsoft.AspNetCore.Authorization;
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
    public class TipoPersonaController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IRepositorioTipoPersona repositorio;

        public TipoPersonaController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.repositorio = new RepositorioTipoPersona(configuration);
        }

        // GET: TipoPersonaController
        public ActionResult Index()
        {
            ViewData["Error"] = TempData["Error"];
            ViewData["Warning"] = TempData["Warning"];
            ViewData["Success"] = TempData["Success"];

            var lista = repositorio.getAll();
            ViewData[nameof(TipoPersona)] = lista;
            ViewData["Title"] = nameof(TipoPersona);
            return View(lista);
        }



        // GET: TipoPersonaController/Details/5
        public ActionResult Details(int id)
        {
            var e = repositorio.getById(id);
            //ViewData[nameof(TipoPersona)] = e;
            return View(e);
        }

        // GET: TipoPersonaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoPersonaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoPersona e)
        {
            try
            {
                repositorio.Create(e);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TipoPersonaController/Edit/5
        public ActionResult Edit(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }

        // POST: TipoPersonaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TipoPersona e)
        {
            try
            {
                repositorio.Edit(e);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        [Authorize(Policy = "Administrador")]
        // GET: TipoPersonaController/Delete/5
        public ActionResult Delete(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }
        [Authorize(Policy = "Administrador")]
        // POST: TipoPersonaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, TipoPersona e)
        {
            try
            {
                repositorio.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                TempData["Error"] = ex.Number == 547 ? "No se puede borrar el tipo Persona porque esta utilizado" : "Ocurrio un error.";
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

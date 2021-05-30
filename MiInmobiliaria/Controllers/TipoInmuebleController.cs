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
    public class TipoInmuebleController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IRepositorioTipoInmueble repositorio;

        public TipoInmuebleController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.repositorio = new RepositorioTipoInmueble(configuration);
        }

        // GET: TipoInmuebleController
        public ActionResult Index()
        {
            ViewData["Error"] = TempData["Error"];
            ViewData["Warning"] = TempData["Warning"];
            ViewData["Success"] = TempData["Success"];

            var lista = repositorio.getAll();
            ViewData[nameof(TipoInmueble)] = lista;
            ViewData["Title"] = nameof(TipoInmueble);
            return View(lista);
        }

        // GET: TipoInmuebleController/Details/5
        public ActionResult Details(int id)
        {
            var e = repositorio.getById(id);
            //ViewData[nameof(TipoInmueble)] = e;
            return View(e);
        }

        // GET: TipoInmuebleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoInmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoInmueble e)
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

        // GET: TipoInmuebleController/Edit/5
        public ActionResult Edit(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }

        // POST: TipoInmuebleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TipoInmueble e)
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
        // GET: TipoInmuebleController/Delete/5
        public ActionResult Delete(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }
        [Authorize(Policy = "Administrador")]
        // POST: TipoInmuebleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, TipoInmueble e)
        {
            try
            {
                repositorio.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                TempData["Error"] = ex.Number == 547 ? "No se puede borrar el tipo de Inmueble porque esta utilizado" : "Ocurrio un error.";
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

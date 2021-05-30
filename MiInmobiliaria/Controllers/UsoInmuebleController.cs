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
    public class UsoInmuebleController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IRepositorioUsoInmueble repositorio;

        public UsoInmuebleController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.repositorio = new RepositorioUsoInmueble(configuration);
        }

        // GET: UsoInmuebleController
        public ActionResult Index()
        {
            ViewData["Error"] = TempData["Error"];
            ViewData["Warning"] = TempData["Warning"];
            ViewData["Success"] = TempData["Success"];

            var lista = repositorio.getAll();
            ViewData[nameof(UsoInmueble)] = lista;
            ViewData["Title"] = nameof(UsoInmueble);
            return View(lista);
        }



        // GET: UsoInmuebleController/Details/5
        public ActionResult Details(int id)
        {
            var e = repositorio.getById(id);
            //ViewData[nameof(UsoInmueble)] = e;
            return View(e);
        }

        // GET: UsoInmuebleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsoInmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsoInmueble e)
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



        // GET: UsoInmuebleController/Edit/5
        public ActionResult Edit(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }

        // POST: UsoInmuebleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UsoInmueble e)
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
        // GET: UsoInmuebleController/Delete/5
        public ActionResult Delete(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }
        [Authorize(Policy = "Administrador")]
        // POST: UsoInmuebleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, UsoInmueble e)
        {
            try
            {
                repositorio.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                TempData["Error"] = ex.Number == 547 ? "No se puede borrar el uso del inmueble porque esta utilizado" : "Ocurrio un error.";
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

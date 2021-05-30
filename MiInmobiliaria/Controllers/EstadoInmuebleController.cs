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
    public class EstadoInmuebleController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioEstadoInmueble repositorio;
        public EstadoInmuebleController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.repositorio = new RepositorioEstadoInmueble(configuration);
        }

        // GET: EstadoInmuebleController
        public ActionResult Index()
        {
            var lista = repositorio.Listar();
            ViewData[nameof(EstadoInmueble)] = lista;
            ViewData["Title"] = nameof(EstadoInmueble);
            return View(lista);
        }

        // GET: EstadoInmuebleController/Details/5
        public ActionResult Details(int id)
        {
            var e = repositorio.Obtener(id);
            //ViewData[nameof(EstadoInmueble)] = e;
            return View(e);
        }

        // GET: EstadoInmuebleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EstadoInmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EstadoInmueble e)
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

        // GET: EstadoInmuebleController/Edit/5
        public ActionResult Edit(int id)
        {
            var e = repositorio.Obtener(id);
            return View(e);
        }

        // POST: EstadoInmuebleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EstadoInmueble e)
        {
            try
            {
                repositorio.Editar(e);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EstadoInmuebleController/Delete/5
        public ActionResult Delete(int id)
        {
            var e = repositorio.Obtener(id);
            return View(e);
        }

        // POST: EstadoInmuebleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, EstadoInmueble e)
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

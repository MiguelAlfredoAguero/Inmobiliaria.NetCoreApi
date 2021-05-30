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
    public class InmuebleController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly IRepositorioInmueble repositorio;
        private readonly IRepositorioPropietario repositorioPropietario;
        private readonly IRepositorioAgencia repositorioAgencia;
        private readonly IRepositorioUsoInmueble repositorioUsoInmueble;
        private readonly IRepositorioTipoInmueble repositorioTipoInmueble;
        private readonly Utils utils;

        public InmuebleController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = new RepositorioInmueble(configuration);

            this.repositorioPropietario = new RepositorioPropietario(configuration);
            this.repositorioAgencia = new RepositorioAgencia(configuration);
            this.repositorioUsoInmueble = new RepositorioUsoInmueble(configuration);
            this.repositorioTipoInmueble = new RepositorioTipoInmueble(configuration);
            this.utils = new Utils(configuration, environment);
        }

        
        // GET: InmuebleController
        public ActionResult Index()
        {
            ViewData["Error"] = TempData["Error"];
            ViewData["Warning"] = TempData["Warning"];
            ViewData["Success"] = TempData["Success"];

            var lista = repositorio.getAll();
            ViewBag.filtroDesocupado = false;
            ViewBag.filtroDisponible = false;
            ViewBag.filtroPropietario = false;
            ViewBag.filtroAgencia = false;
            ViewData["Error"] = TempData["Error"];
            return View(lista);
        }



        // GET: InmuebleController/Details/5
        public ActionResult Details(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }



        // GET: InmuebleController/Create
        public ActionResult Create(int Id)
        {
            if (Id == 0)
                ViewBag.Propietarios = repositorioPropietario.getAll();
            else { 
                ViewBag.Propietarios = repositorioPropietario.getAll(Id);
            }
            ViewBag.Agencias = repositorioAgencia.getAll();
            ViewBag.Usos = repositorioUsoInmueble.getAll();
            ViewBag.Tipos = repositorioTipoInmueble.getAll();
            return View();
        }

        // POST: InmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble e)
        {
            try
            {
                e.Avatar = "";
                e.Id = repositorio.Create(e);
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



        // GET: InmuebleController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Propietarios = repositorioPropietario.getAll();
            ViewBag.Agencias = repositorioAgencia.getAll();
            ViewBag.Usos = repositorioUsoInmueble.getAll();
            ViewBag.Tipos = repositorioTipoInmueble.getAll();
            var e = repositorio.getById(id);
            return View(e);
        }

        // POST: InmuebleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble e)
        {
            try
            {
                if (e.Avatar == null) {
                    e.Avatar = "";
                }

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
        // GET: InmuebleController/Delete/5
        public ActionResult Delete(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }
        [Authorize(Policy = "Administrador")]
        // POST: InmuebleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inmueble e)
        {
            try
            {
                repositorio.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                TempData["Error"] = ex.Number == 547 ? "No se puede borrar el Inmueble porque esta utilizado" : "Ocurrio un error.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrio un error." + ex.ToString();
                return RedirectToAction(nameof(Index));
            }
        }



        public ActionResult getByDisponible()
        {
            var list = repositorio.getAllDisponibles();
            ViewBag.filtroDesocupado = false;
            ViewBag.filtroDisponible = true;
            ViewBag.filtroPropietario = false;
            ViewBag.filtroAgencia = false;
            return View("Index", list);
        }


        public ActionResult getByPropietario(int id)
        {
            ViewBag.Propietario = repositorioPropietario.getById(id);
            var list = repositorio.getAll( (Propietario)ViewBag.Propietario );
            ViewBag.filtroDesocupado = false;
            ViewBag.filtroDisponible = false;
            ViewBag.filtroPropietario = true;
            ViewBag.filtroAgencia = false;
            return View("Index", list);
        }

        public ActionResult getByAgencia(int id)
        {
            ViewBag.Agencia = repositorioAgencia.getById(id);
            var list = repositorio.getAll((Agencia)ViewBag.Agencia);
            ViewBag.filtroDesocupado = false;
            ViewBag.filtroDisponible = false;
            ViewBag.filtroPropietario = false;
            ViewBag.filtroAgencia = true;
            return View("Index", list);
        }

        // GET: ContratoController
        public ActionResult Desocupados(DateTime desde, DateTime hasta)
        {
            if (desde <= hasta)
            {
                var lista = repositorio.getDesocupados(desde, hasta);
                ViewBag.filtroDesocupado = true;
                ViewBag.filtroDisponible = false;
                ViewBag.filtroPropietario = false;
                ViewBag.filtroAgencia = false;
                return View("Index", lista);
            } else
            {
                TempData["Error"] = "Error en las fechas desde hasta. Desde no debe ser mayor que hasta.";
                return RedirectToAction(nameof(Index));
            }

        }


        public ActionResult Busqueda()
        {
            ViewBag.Usos = repositorioUsoInmueble.getAll();
            ViewBag.Tipos = repositorioTipoInmueble.getAll();
            ViewBag.filtroDesocupado = false;
            var lista = repositorio.getAll();
            return View(lista);
        }
        [HttpPost]
        public ActionResult Busqueda(int UsoInmuebleId, int TipoInmuebleId, int ambientes, DateTime desde, DateTime hasta, decimal minimo, decimal maximo)
        {
            try
            {
                var lista = repositorio.Busqueda(UsoInmuebleId, TipoInmuebleId, ambientes, desde, hasta, minimo, maximo);
                ViewBag.Usos = repositorioUsoInmueble.getAll();
                ViewBag.Tipos = repositorioTipoInmueble.getAll();
                ViewBag.filtroDesocupado = true;
                return View(lista);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrio un error." + ex.ToString();
                return RedirectToAction(nameof(Index));
            }
        }

    }
}

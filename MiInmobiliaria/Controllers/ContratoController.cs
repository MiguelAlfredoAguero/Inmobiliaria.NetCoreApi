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
    public class ContratoController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly RepositorioContrato repositorio;
        private readonly RepositorioInmueble repositorioInmueble;
        private readonly IRepositorioInquilino repositorioInquilino;
        private readonly IRepositorioGarante repositorioGarante;

        private Inmueble inmueble = new Inmueble();

        public ContratoController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = new RepositorioContrato(configuration);

            this.repositorioInmueble = new RepositorioInmueble(configuration);
            this.repositorioInquilino = new RepositorioInquilino(configuration);
            this.repositorioGarante = new RepositorioGarante(configuration);
        }

        // GET: ContratoController
        [Authorize]
        public ActionResult Index()
        {
            ViewData["Error"] = TempData["Error"];
            ViewData["Warning"] = TempData["Warning"];
            ViewData["Success"] = TempData["Success"];

            var lista = repositorio.getAll();
            ViewBag.filtroVigente = false;
            ViewBag.filtroInmueble = false;
            return View(lista);
        }



        // GET: ContratoController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }



        // GET: ContratoController/Create
        [Authorize]
        public ActionResult Create(int Id)
        {
            if (Id != 0) {
                var e = repositorioInmueble.getById(Id);
                if ( !e.Disponible ) {
                    TempData["Error"] = "El inmueble seleccioando NO esta disponible.";
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Inmueble = e;
            } else
            {
                ViewBag.Inmuebles = repositorioInmueble.getAllDisponibles();
            }
            ViewBag.Inquilinos = repositorioInquilino.getAll();
            ViewBag.Garantes = repositorioGarante.getAll();
            return View();
        }

        // POST: ContratoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(int Id, Contrato e)
        {
            try
            {
                if (repositorioInmueble.getDesocupado(e.Desde, e.Hasta, Id) == null)
                {
                    e.Inmueble = repositorioInmueble.getById(Id);
                    e.InmuebleId = e.Inmueble.Id;
                    e.Precio = e.Inmueble.Precio;
                    e.Id = repositorio.Create(e);
                    TempData["Success"] = "Contrato creado con éxito.";
                    return RedirectToAction(nameof(Index));
                } else
                {
                    TempData["Error"] = "El inmueble NO esta desocupado entre las fechas seleccionadas. " + e.Desde + e.Hasta;
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrio un error." + ex.ToString();
                return View();
            }
        }



        // GET: ContratoController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            ViewBag.Inmuebles = repositorioInmueble.getAll();
            ViewBag.Inquilinos = repositorioInquilino.getAll();
            ViewBag.Garantes = repositorioGarante.getAll();
            var e = repositorio.getById(id);
            return View(e);
        }

        // POST: ContratoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Contrato e)
        {
            ViewBag.filtroVigente = false;
            try
            {
                repositorio.Edit(e);
                ViewBag.Inmuebles = repositorioInmueble.getAll();
                ViewBag.Inquilinos = repositorioInquilino.getAll();
                ViewBag.Garantes = repositorioGarante.getAll();
                TempData["Success"] = "Contrato editado con éxito.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrio un error." + ex.ToString();
                return RedirectToAction(nameof(Index));
            }
        }



        [Authorize(Policy = "Administrador")]
        // GET: ContratoController/Delete/5
        public ActionResult Delete(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }
        [Authorize(Policy = "Administrador")]
        // POST: ContratoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Contrato e)
        {
            ViewBag.filtroVigente = false;
            try
            {
                repositorio.Delete(id);
                TempData["Success"] = "Contrato borrado con éxito.";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                TempData["Error"] = ex.Number == 547 ? "No se puede borrar el Contrato porque esta utilizado" : "Ocurrio un error.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrio un error." + ex.ToString();
                return RedirectToAction(nameof(Index));
            }
        }


        // GET: ContratoController
        [Authorize]
        public ActionResult Vigentes()
        {
            var lista = repositorio.getAllVigentes();
            ViewBag.filtroVigente = true;
            ViewBag.filtroInmueble = false;
            return View("Index", lista);
        }




        // GET: ContratoController
        [Authorize]
        public ActionResult Renovar(int Id)
        {
            var e = repositorio.getById(Id);
            TimeSpan difFechas = e.Hasta - e.Desde;
            e.Desde = DateTime.Now;
            e.Hasta = DateTime.Now.AddDays(difFechas.Days);
            return View(e);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Renovar(int Id, Contrato e)
        {
            ViewBag.filtroVigente = false;
            try
            {
                if (repositorioInmueble.getDesocupado(e.Desde, e.Hasta, Id) == null)
                {
                    repositorio.Renovar(e);
                    TempData["Success"] = "Contrato renovado con éxito.";
                    ViewBag.filtroVigente = false;
                    ViewBag.filtroInmueble = false;
                }
                else
                {
                    TempData["Error"] = "Error: Inmueble ocupado entre las fechas dadas.";
                }

                return RedirectToAction(nameof(Index));
            }

            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrio un error." + ex.ToString();
                return RedirectToAction(nameof(Index));
            }
        }




        // GET: ContratoController
        [Authorize]
        public ActionResult getByInmueble(int Id)
        {
            ViewBag.Inmueble = repositorioInmueble.getById(Id);
            var lista = repositorio.getByInmueble(Id);
            ViewBag.filtroVigente = false;
            ViewBag.filtroInmueble = true;
            inmueble = repositorioInmueble.getById(Id);
            return View("Index", lista);
        }



        // GET: ContratoController/Edit/5
        [Authorize]
        public ActionResult Cancelar(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Cancelar(int id, Contrato e)
        {
            if (e.Desde <= e.Hasta && e.Hasta <= DateTime.Now)
            {
                decimal multa;
                TimeSpan difFechas = e.Hasta - e.Desde;
                var contrato = repositorio.getById(id);
                TimeSpan difContrato = contrato.Hasta - contrato.Desde;
                if (difFechas.Days < difContrato.Days / 2)
                    multa = contrato.Precio * 2;
                else
                    multa = contrato.Precio;
                TempData["Warning"] = "Por la cancelación anticipada del contato, debe abonar una multa de $ " + multa;
                ViewBag.filtroVigente = false;
                ViewBag.filtroInmueble = false;
                repositorio.Edit(e);
                return RedirectToAction(nameof(Index));
            } else
            {
                TempData["Error"] = "Error en la fecha de cancelación del contrato.";
                return RedirectToAction(nameof(Index));
            }

        }
    }
}

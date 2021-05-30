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
    public class PagoController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IRepositorioPago repositorio;
        private readonly IRepositorioContrato repositorioContrato;

        public PagoController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.repositorio = new RepositorioPago(configuration);
            this.repositorioContrato = new RepositorioContrato(configuration);
        }


        // GET: PagoController
        public ActionResult Index()
        {
            ViewData["Error"] = TempData["Error"];
            ViewData["Warning"] = TempData["Warning"];
            ViewData["Success"] = TempData["Success"];

            var lista = repositorio.getAll();
            ViewBag.filtroContrato = false;
            return View(lista);
        }



        // GET: PagoController/Details/5
        public ActionResult Details(int id)
        {
            var e = repositorio.getById(id);
            ViewBag.Contrato = repositorioContrato.getById(e.ContratoId);
            return View(e);
        }



        // GET: PagoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PagoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago e)
        {
            try
            {
                e.ContratoId = e.Id;
                e.Id = 0;
                repositorio.Create(e);
                TempData["Success"] = "Pago registrado con éxito.";
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

        public ActionResult RegistrarPago(int Id)
        {
            var e = repositorioContrato.getById(Id);
            ViewBag.numeroUltimoPago = repositorio.numeroUltimoPago(Id);
            ViewBag.Contrato = e;
            return View("Create");
        }



        public ActionResult VerPagos(int Id)
        {
            var e = repositorioContrato.getById(Id);
            ViewBag.filtroContrato = true;
            ViewBag.Contrato = e;
            var lista = repositorio.getAll(Id);
            return View("Index", lista);
        }




        // GET: PagoController/Edit/5
        public ActionResult Edit(int id)
        {
            var e = repositorio.getById(id);
            ViewBag.Contrato = repositorioContrato.getById(e.ContratoId);
            return View(e);
        }

        // POST: PagoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pago e)
        {
            try
            {
                repositorio.Edit(e);
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




        [Authorize(Policy = "Administrador")]
        // GET: PagoController/Delete/5
        public ActionResult Delete(int id)
        {
            var e = repositorio.getById(id);
            ViewBag.Contrato = repositorioContrato.getById(e.ContratoId);
            return View(e);
        }
        [Authorize(Policy = "Administrador")]
        // POST: PagoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Pago e)
        {
            try
            {
                repositorio.Delete(id);
                TempData["Success"] = "Pago borrado con exito.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "No se puede borrar el pago." + ex.Message;
                return View();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs;
using Persisten.Database;
using Service;

namespace GestionAntioquia.Controllers
{
    public class CompanyController : Controller
    {

        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        // GET: hotel

        public async Task<IActionResult> Allies()
        {
            var _aliados = await _companyService.GetAll();

            var _nuevo = DateTime.Now.AddMonths(1);

            var _viewAliados = (from a in _aliados
                                select new PlacesAlliesDto
                                {
                                    PlaceId = a.PlaceId,
                                    Name = a.Name,
                                    UrlName = a.NameUrl,
                                    Contract = a.Contract,
                                    CoverPage = a.CoverPage,
                                    Description = a.Description.Substring(0, 20),
                                    DataCreate = DateTime.Parse(a.CreationDate).AddMonths(1),
                                    New = "nuevo"
                                });

            return View(_viewAliados);
        }

        // GET: hotel/Details/5
        public async Task<ActionResult> Details(string urlName)
        {

            if (urlName == null)
            {
                return NotFound();
            }

            var _detalleHotel = await _companyService.Details(urlName);

            if (_detalleHotel == null)
            {
                return NotFound();
            }

            return View(_detalleHotel);
        }

        // GET: hotel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: hotel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: hotel/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: hotel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: hotel/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: hotel/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
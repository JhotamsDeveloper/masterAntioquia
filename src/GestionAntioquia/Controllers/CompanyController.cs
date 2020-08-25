using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        [TempData]
        public string _StatusMessaje { get; set; }

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        // GET: hotel

        [Route("empresas")]
        public async Task<IActionResult> Allies()
        {
            var _aliados = await _companyService.GetAll();

            var _viewAliados = from a in _aliados
                                select new PlacesAlliesDto
                                {
                                    PlaceId = a.PlaceId,
                                    Name = a.Name,
                                    City = a.City,
                                    urban = a.urban,
                                    UrlName = a.NameUrl,
                                    Contract = a.Contract,
                                    SquareCover = a.SquareCover,
                                    Description = a.Description.Substring(0, 100),
                                    DataCreate = DateTime.Parse(a.CreationDate).AddMonths(1),
                                    New = "nuevo",
                                    NameCategory = a.Category.Name
                                };

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

            string _urbano;

            if (_detalleHotel.urban){_urbano = "Urbana";}else{
                _urbano = "rural";};

            ViewData["Urbano"] = _urbano;

            if (_StatusMessaje != "")
            {
                ViewData["successful"] = _StatusMessaje;
            }

            if (_detalleHotel == null)
            {
                return NotFound();
            }

            return View(_detalleHotel);
        }

        // GET: hotel/Details/5
        public async Task<ActionResult> Products(string urlName)
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

        [Authorize(Roles = "UserApp, Admin")]
        public async Task<ActionResult> Reviews(ReviewsCreateDto model)
        {

            if (ModelState.IsValid)
            {
                await _companyService.CreateReviews(model);
                return RedirectToAction(nameof(Index));
            }
            _StatusMessaje = "show";
            return View("Details");

        }

    }
}
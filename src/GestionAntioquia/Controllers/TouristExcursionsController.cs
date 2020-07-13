using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persisten.Database;
using Service;

namespace GestionAntioquia.Controllers
{
    public class TouristExcursionsController : Controller
    {
        private readonly IGenericServicio _genericServicio;

        public TouristExcursionsController(IGenericServicio genericServicio)
        {
            _genericServicio = genericServicio;
        }

        public IActionResult Index()
        {

            return RedirectToRoute(new { controller = "TouristExcursions", action = "StatedNotFound" });
        }

        public async Task<IActionResult> StatedNotFound()
        {
            ViewData["Gracias"] = "Gracias";
            ViewData["Details"] = "Esta página esta en construcción";

            ViewData["Product"] = await _genericServicio.NewsList(5);
            Response.StatusCode = 404;

            return PartialView("_NotFound");
        }

    }
}
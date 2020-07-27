using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Persisten.Database;
using Service;

namespace GestionAntioquia.Controllers
{
    public class TouristExcursionsController : Controller
    {
        private readonly IGenericServicio _genericServicio;
        private readonly ICategoryService _categoryService;

        public TouristExcursionsController(IGenericServicio genericServicio,
                ICategoryService categoryService)
        {
            _genericServicio = genericServicio;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return RedirectToRoute(new { controller = "TouristExcursions", action = "StatedNotFound" });
        }


        public async Task<IActionResult> StatedNotFound()
        {
            //Para buscar
            ViewData["Categories"] = new SelectList(await _categoryService.CategorySelectGetAll(), "CategoryId", "Name");

            ViewData["Gracias"] = "Gracias";
            ViewData["Details"] = "Esta página esta en construcción";

            ViewData["Product"] = await _genericServicio.NewsList(5);
            Response.StatusCode = 404;

            return PartialView("_NotFound404");
        }


    }
}
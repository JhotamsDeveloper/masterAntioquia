using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace GestionAntioquia.Controllers
{
    public class WhatToDoController : Controller
    {
        private readonly IGenericServicio _genericServicio;

        public WhatToDoController(IGenericServicio genericServicio)
        {
            _genericServicio = genericServicio;
        }

        public IActionResult Index()
        {

            return RedirectToRoute(new { controller = "WhatToDo", action = "StatedNotFound" });
        }

        public async Task<IActionResult> StatedNotFound()
        {

            ViewData["Product"] = await _genericServicio.NewsList(5);
            Response.StatusCode = 404;

            return View("NotFound");
        }
    }
}
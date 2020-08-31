using GestionAntioquia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model.DTOs;
using Persisten.Database;
using Service;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAntioquia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBlogService _blogService;
        private readonly IPlaceService _placeService;
        private readonly IGenericServicio _genericServicio;

        [TempData]
        public string _StatusMessaje { get; set; }

        public HomeController(
                ApplicationDbContext context,
                IBlogService blogService,
                IPlaceService placeService,
                IGenericServicio genericServicio)
        {
            _context = context;
            _blogService = blogService;
            _placeService = placeService;
            _genericServicio = genericServicio;
        }

        public async Task<IActionResult> Index()
        {
            //Logos
            ViewData["Logos"] = await _placeService.Logos();

            return View(await _blogService.Blog(6));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "UserApp, Admin")]
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        // GET: Contact
        public IActionResult Contact()
        {
            if (_StatusMessaje != null)
            {
                ViewData["successful"] = "show";
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendEmail(AtributeContact model)
        {
            var _message = $"Hola soy {model.Name} <br> {model.Message}";
            
            var _send = _genericServicio.Execute(model.Subject, _message, model.Email);

            if (_send != null)
            {
                _StatusMessaje = "Hemos recibido tu mensaje";
            }

            return RedirectToAction(nameof(Contact));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

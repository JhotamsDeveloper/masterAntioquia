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
using System.Linq;
using System.Threading.Tasks;

namespace GestionAntioquia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBlogService _blogService;
        private readonly IPlaceService _placeService;

        public HomeController(
                ApplicationDbContext context,
                IBlogService blogService,
                IPlaceService placeService)
        {
            _context = context;
            _blogService = blogService;
            _placeService = placeService;
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

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

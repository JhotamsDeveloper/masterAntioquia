using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GestionAntioquia.Models;
using Service;
using Persisten.Database;
using Microsoft.EntityFrameworkCore;
using Model;

namespace GestionAntioquia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBlogService _blogService;

        public HomeController(ApplicationDbContext context,
                IBlogService blogService)
        {
            _context = context;
            _blogService = blogService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _blogService.Blog(6));
        }

        public async Task<IActionResult> Search(
        string searchString,
        string currentFilter,
        int? pag)
        {

            if (searchString != null)
            {
                pag = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var _query = from a in _context.Events
                         .Where(x => x.Category.Name == "Blog")
                         select a;

            //Para la busqueda
            if (!String.IsNullOrEmpty(searchString))
            {
                _query = _query.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper())
                                       || s.Author.ToUpper().Contains(searchString.ToUpper()));
            }

            int pageSize = 6;
            return PartialView(await PaginatedList<Event>.CreateAsync(_query.AsNoTracking(), pag ?? 1, pageSize));

        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
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

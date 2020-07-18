using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using GestionAntioquia.Models;
using Service;
using Persisten.Database;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTOs;


namespace GestionAntioquia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IBlogService _blogService;

        public HomeController(ApplicationDbContext context,
                ICategoryService categoryService,
                IBlogService blogService)
        {
            _context = context;
            _categoryService = categoryService;
            _blogService = blogService;
        }

        public async Task<IActionResult> Index()
        {
            var _listCategory = new SearchDto();
            _listCategory.CategoriesGetAll.ToList();

            ViewData["Categories"] = new SelectList(await _categoryService.GetAll(), "CategoryId", "Name");

            return View(await _blogService.Blog(6));
        }

        public async Task<IActionResult> Search(
            string stringSelect,
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

            var _product = from a in _context.Products
                         where a.Statud == true
                         select new SearchDto
                         {
                             SearchDate = a.CreationDate,
                             Name = a.Name,
                             NameUrl = a.ProductUrl,
                             SquareCover = a.SquareCover,
                         };

            var _place = from a in _context.Places
                         where a.State == true
                         select new SearchDto
                         {
                             SearchDate = DateTime.Parse(a.CreationDate),
                             Name = a.Name,
                             NameUrl = a.NameUrl,
                             SquareCover = a.SquareCover,
                         };

            var _event = from a in _context.Events
                         where a.State == true
                         select new SearchDto
                         {
                            SearchDate = a.CreationDate,
                            Name = a.Name,
                            NameUrl = a.EventUrl,
                            SquareCover = a.SquareCover,
                         };

            //Para la busqueda
            if (!String.IsNullOrEmpty(searchString))
            {
                _product = _product.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()));
                _place = _place.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()));
                _event = _event.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()));
            }


            int pageSize = 6;
            return PartialView(await PaginatedList<SearchDto>.CreateAsync(_product.AsNoTracking(), pag ?? 1, pageSize));

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

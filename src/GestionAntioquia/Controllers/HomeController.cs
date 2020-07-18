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
        private readonly IGenericServicio _genericServicio;

        public HomeController(ApplicationDbContext context,
                ICategoryService categoryService,
                IBlogService blogService,
                IGenericServicio genericServicio)
        {
            _context = context;
            _categoryService = categoryService;
            _blogService = blogService;
            _genericServicio = genericServicio;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Categories"] = new SelectList(await _categoryService.CategorySelectGetAll(), "CategoryId", "Name");

            return View(await _blogService.Blog(6));
        }

        public async Task<IActionResult> Search(
            string stringSelect,
            string searchString,
            string currentFilter,
            int? pag)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["stringSelect"] = stringSelect;

            if (searchString != null){pag = 1;}else{searchString = currentFilter;}

            string _category = stringSelect;
            
            if (!String.IsNullOrEmpty(_category))
            {
                string[] _categories = new string[4] { "1", "2", "3", "4" };

                //Categoria Hotel
                if (_categories[0] == _category)
                {
                    var _query = from a in _context.Places
                                 where a.State == true
                                 select new SearchDto
                                 {
                                     SearchDate = DateTime.Parse(a.CreationDate),
                                     Name = a.Name,
                                     NameUrl = a.NameUrl,
                                     SquareCover = a.SquareCover,
                                 };

                    //Para la busqueda
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        _query = _query.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()));
                    }

                    if (_query.Count() <= 0)
                    {
                        ViewData["Gracias"] = "Vaya";
                        ViewData["Details"] = "No Hemos encontrado nada que coincida";

                        ViewData["Product"] = await _genericServicio.NewsList(5);
                        Response.StatusCode = 404;

                        return PartialView("_NotFound");
                    }

                    int pageSize = 6;
                    return PartialView(await PaginatedList<SearchDto>.CreateAsync(_query.AsNoTracking(), pag ?? 1, pageSize));
                }


                
                //Categoria Restaurante
                if (_categories[1] == _category)
                {
                    var _query = from a in (_context.Products
                                           .Include(x=>x.Place)
                                           .ThenInclude(c=>c.Category))
                                    where a.Statud == true && a.Place.Category.Name == "Restaurante"
                                    select new SearchDto
                                    {
                                        SearchDate = a.CreationDate,
                                        Name = a.Name,
                                        NameUrl = a.ProductUrl,
                                        SquareCover = a.SquareCover,
                                    };

                                //Para la busqueda
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        _query = _query.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()));
                    }

                    if (_query.Count() <= 0)
                    {
                        ViewData["Gracias"] = "Vaya";
                        ViewData["Details"] = "No Hemos encontrado nada que coincida";

                        ViewData["Product"] = await _genericServicio.NewsList(5);
                        Response.StatusCode = 404;

                        return PartialView("_NotFound");
                    }

                    int pageSize = 6;
                    return PartialView(await PaginatedList<SearchDto>.CreateAsync(_query.AsNoTracking(), pag ?? 1, pageSize));
                }

                //Categoria Tienda
                if (_categories[2] == _category)
                {
                    var _query = from a in (_context.Products
                                           .Include(x => x.Place)
                                           .ThenInclude(c => c.Category))
                                 where a.Statud == true && a.Place.Category.Name == "Tienda"
                                 select new SearchDto
                                 {
                                     SearchDate = a.CreationDate,
                                     Name = a.Name,
                                     NameUrl = a.ProductUrl,
                                     SquareCover = a.SquareCover,
                                 };

                    //Para la busqueda
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        _query = _query.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()));
                    }

                    if (_query.Count() <= 0)
                    {
                        ViewData["Gracias"] = "Vaya";
                        ViewData["Details"] = "No Hemos encontrado nada que coincida";

                        ViewData["Product"] = await _genericServicio.NewsList(5);
                        Response.StatusCode = 404;

                        return PartialView("_NotFound");
                    }

                    int pageSize = 6;
                    return PartialView(await PaginatedList<SearchDto>.CreateAsync(_query.AsNoTracking(), pag ?? 1, pageSize));
                }

                //Categoria Blog
                if (_categories[3] == _category)
                {
                    var _query = from a in _context.Events
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
                        _query = _query.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()));
                    }

                    if (_query.Count() <= 0)
                    {
                        ViewData["Gracias"] = "Vaya";
                        ViewData["Details"] = "No Hemos encontrado nada que coincida";

                        ViewData["Product"] = await _genericServicio.NewsList(5);
                        Response.StatusCode = 404;

                        return PartialView("_NotFound");
                    }

                    int pageSize = 6;
                    return PartialView(await PaginatedList<SearchDto>.CreateAsync(_query.AsNoTracking(), pag ?? 1, pageSize));
                }

            }
            else
            {
                ViewData["Gracias"] = "Vaya";
                ViewData["Details"] = "No Hemos encontrado nada que coincida";

                ViewData["Product"] = await _genericServicio.NewsList(5);
                Response.StatusCode = 404;

                return PartialView("_NotFound");
            }


            return View("Index");
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

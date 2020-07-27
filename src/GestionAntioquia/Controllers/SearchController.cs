using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAntioquia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model.DTOs;
using Persisten.Database;
using Service;

namespace GestionAntioquia.Controllers
{
    public class SearchController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IGenericServicio _genericServicio;
        private readonly ICategoryService _categoryService;

        public SearchController(
                ApplicationDbContext context,
                IGenericServicio genericServicio,
                ICategoryService categoryService)
        {
            _context = context;
            _genericServicio = genericServicio;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Search(
            string stringSelect,
            string searchString,
            string currentFilter,
            int? pag)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["stringSelect"] = stringSelect;

            if (searchString != null) { pag = 1; } else { searchString = currentFilter; }

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
                        ViewData["Categories"] = new SelectList(await _categoryService.CategorySelectGetAll(), "CategoryId", "Name");

                        ViewData["Gracias"] = "Ups!";
                        ViewData["Details"] = "No hemos encontrado nada";

                        ViewData["Product"] = await _genericServicio.NewsList(5);
                        Response.StatusCode = 404;

                        return PartialView("_NotFound404");
                    }

                    int pageSize = 6;
                    return PartialView(await PaginatedList<SearchDto>.CreateAsync(_query.AsNoTracking(), pag ?? 1, pageSize));
                }



                //Categoria Restaurante
                if (_categories[1] == _category)
                {
                    var _query = from a in (_context.Products
                                           .Include(x => x.Place)
                                           .ThenInclude(c => c.Category))
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
                        ViewData["Categories"] = new SelectList(await _categoryService.CategorySelectGetAll(), "CategoryId", "Name");

                        ViewData["Gracias"] = "Ups!";
                        ViewData["Details"] = "No hemos encontrado nada";

                        ViewData["Product"] = await _genericServicio.NewsList(5);
                        Response.StatusCode = 404;

                        return PartialView("_NotFound404");
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
                        ViewData["Categories"] = new SelectList(await _categoryService.CategorySelectGetAll(), "CategoryId", "Name");

                        ViewData["Gracias"] = "Ups!";
                        ViewData["Details"] = "No hemos encontrado nada";

                        ViewData["Product"] = await _genericServicio.NewsList(5);
                        Response.StatusCode = 404;

                        return PartialView("_NotFound404");
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
                        ViewData["Categories"] = new SelectList(await _categoryService.CategorySelectGetAll(), "CategoryId", "Name");

                        ViewData["Gracias"] = "Ups!";
                        ViewData["Details"] = "No hemos encontrado nada";

                        ViewData["Product"] = await _genericServicio.NewsList(5);
                        Response.StatusCode = 404;

                        return PartialView("_NotFound404");
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

    }
}
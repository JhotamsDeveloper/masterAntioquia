using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GestionAntioquia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTOs;
using Persisten.Database;
using Service;

namespace GestionAntioquia.Controllers
{
    public class BlogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBlogService _blogService;
        private readonly IGenericServicio _genericServicio;

        public BlogsController(
            ApplicationDbContext context,
            IGenericServicio genericServicio,
            IBlogService blogService)
        {
            _context = context;
            _blogService = blogService;
            _genericServicio = genericServicio;
        }

        #region "BackEnd"
        // GET: Blogs
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(
            string sortOrder, 
            string searchString,
            string currentFilter,
            int? pageNumber)
        {

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var _query = from q in _context.Events
                         .Where(x=>x.Category.Name == "Blog")
                         select q;

            //Para la busqueda
            if (!String.IsNullOrEmpty(searchString))
            {
                _query = _query.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper())
                                       || s.Author.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    _query = _query
                        .OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    _query = _query
                        .OrderBy(s => s.Author);
                    break;
                case "date_desc":
                    _query = _query
                        .OrderByDescending(s => s.UpdateDate);
                    break;
                default:
                    _query = _query
                        .OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Event>.CreateAsync(_query.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        // GET: Blogs/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _event = await _blogService.Details(id);

            var _modelo = new BlogView
            {
                EventId = _event.EventId,
                Name = _event.Name,
                BlogUrl = _event.BlogUrl,
                Description = _event.Description,
                Author = _event.Author,
                CoverPage = _event.CoverPage,
                SquareCover = _event.SquareCover,
                UpdateDate = _event.UpdateDate.ToString(),
                State = _event.State
            };

            if (_modelo == null)
            {
                return NotFound();
            }

            ViewData["CoverPage"] = _event.CoverPage;
            ViewData["Edit"] = true;
            return View(_modelo);
        }

        // GET: Blogs/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateDto model)
        {
            if (ModelState.IsValid)
            {
                var _urlName = _blogService.DuplicaName(model.Name);

                if (_urlName)
                {
                    ViewData["DuplicaName"] = $"El Nombre {model.Name} ya ha sido utilizado, cambielo";
                    return View(model);
                }

                await _blogService.Create(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Blogs/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _event = await _blogService.GetById(id);
            if (_event == null)
            {
                return NotFound();
            }

            var _blog = new BlogEditDto
            {
                EventId = _event.EventId,
                Name = _event.Name,
                Description = _event.Description,
                Author = _event.Author,
                UpdateDate = Convert.ToDateTime(_event.UpdateDate),
                State = _event.State
            };

            ViewData["CoverPage"] = _event.CoverPage;
            return View(_blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogEditDto _model)
        {
            if (id != _model.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _blogService.Edit(id, _model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(_model.EventId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(_model);

        }

        // GET: Blogs/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _event = await _blogService.GetById(id);

            if (_event == null)
            {
                return NotFound();
            }
            ViewData["CoverPage"] = _event.CoverPage;
            return View(_event);
        }

        // POST: Blogs/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var _event = await _blogService.GetById(id);

            var _id = _event.EventId;
            var _squareCover = _event.SquareCover;
            var _cove = _event.CoverPage;

            await _blogService.DeleteConfirmed(_id, _squareCover, _cove);

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int? id)
        {
            return _blogService.BlogExists(id);
        }

        #endregion

        #region "FrontEnd"
        [Route("/blog/")]
        public async Task<IActionResult> Blog(
            string currentFilter,
            string searchString,
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

            var _query = from q in _context.Events
             .Where(x =>x.State == true && x.Category.Name == "Blog")
                         select q;

            int pageSize = 9;
            return View(await PaginatedList<Event>.CreateAsync(_query.AsNoTracking(), pag ?? 1, pageSize));

            //var _blog = _blogService.Blog();

            //   return View(await _blog);
        }

        // GET: Blogs/Details/5
        [Route("/blog/{name}")]
        public async Task<IActionResult> Details(string name)
        {
            if (name == null)
            {
                return NotFound();
            }
            
            var _detail = await _blogService.Details(name);

            var _producGuid = await _genericServicio.NewsList(5);

            NumberFormatInfo nfi = new CultureInfo("es-CO", false).NumberFormat;
            nfi = (NumberFormatInfo)nfi.Clone();
            nfi.CurrencySymbol = "$";


            var _listProduct = (from a in _producGuid
                                     select new ProductsView
                                     {
                                         ProductId = a.ProductId,
                                         Name = a.Name,
                                         ProductUrl = a.ProductUrl,
                                         CoverPage = a.CoverPage,
                                         SquareCover = a.SquareCover,
                                         Description = a.Description,
                                         PriceWhitIncrement = string.Format(nfi, "{0:C0}", a.Price + a.Increments),
                                         Discounts = a.Discounts,
                                         ProductWithDiscounts = string.Format(nfi, "{0:C0}", (a.Price + a.Increments) - ((a.Price + a.Increments) * a.Discounts / 100)),
                                         Statud = a.Statud,
                                         PersonNumber = a.PersonNumber,
                                         Place = a.Place

                                     });


            var _modelo = new BlogView
            {
                EventId = _detail.EventId,
                Name = _detail.Name,
                BlogUrl = _detail.BlogUrl,
                Description = _detail.Description,
                Author = _detail.Author,
                CoverPage = _detail.CoverPage,
                SquareCover = _detail.SquareCover,
                UpdateDate = _detail.UpdateDate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                State = _detail.State,
                Products = _listProduct.ToList()
            };


            if (_modelo == null)
            {
                return NotFound();
            }

            ViewData["Edit"] = false;
            return View(_modelo);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAntioquia.Models;
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

        public BlogsController(
            ApplicationDbContext context,
            IBlogService blogService)
        {
            _context = context;
            _blogService = blogService;
        }

    #region "BackEnd"
        // GET: Blogs
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
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _event = await _blogService.Details(id);

            if (_event == null)
            {
                return NotFound();
            }

            ViewData["CoverPage"] = _event.CoverPage;
            return View(_event);
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateDto model)
        {
            if (ModelState.IsValid)
            {
                await _blogService.Create(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Blogs/Edit/5
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
                UpdateDate = _event.UpdateDate,
                State = _event.State
            };

            ViewData["CoverPage"] = _event.CoverPage;
            return View(_blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            
            var _event = await _blogService.Details(name);

            if (_event == null)
            {
                return NotFound();
            }

            ViewData["CoverPage"] = _event.CoverPage;
            return View(_event);
        }

        #endregion
    }
}

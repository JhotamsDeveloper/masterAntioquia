using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model.DTOs;
using Persisten.Database;
using Service;

namespace GestionAntioquia.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenericServicio _genericServicio;
        private readonly ICategoryService _categoryService;
        private readonly IEventService _evantService;

        public EventsController(
                            ApplicationDbContext context,
                            IGenericServicio genericServicio,
                            ICategoryService categoryService,
                            IEventService eventService)
        {
            _context = context;
            _genericServicio = genericServicio;
            _categoryService = categoryService;
            _evantService = eventService;
        }

        #region "BACKEND"
        public async Task<IActionResult> Index()
        {
            return View(await _evantService.GetAll());
        }

        // GET: Evant/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _event = await _evantService.Details(id);

            if (_event == null)
            {
                return NotFound();
            }

            ViewData["CoverPage"] = _event.CoverPage;
            return View(_event);
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            ViewData["places"] = new SelectList(
                _context.Places
                .Where(x=>x.State == true), "PlaceId", "Name");
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateDto model)
        {
            if (ModelState.IsValid)
            {
                await _evantService.Create(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Event/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _event = await _evantService.GetById(id);
            if (_event == null)
            {
                return NotFound();
            }

            var _blog = new EventEditDto
            {
                EventId = _event.EventId,
                Name = _event.Name,
                Description = _event.Description,
                UpdateDate = _event.UpdateDate,
                State = _event.State
            };

            ViewData["CoverPage"] = _event.CoverPage;
            return View(_blog);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventEditDto _model)
        {
            if (id != _model.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _evantService.Edit(id, _model);
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

            var _event = await _evantService.GetById(id);

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
            var _event = await _evantService.GetById(id);

            var _id = _event.EventId;
            var _squareCover = _event.SquareCover;
            var _cove = _event.CoverPage;

            await _evantService.DeleteConfirmed(_id, _squareCover, _cove);

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int? id)
        {
            return _evantService.EventExists(id);
        }
        #endregion

        #region "FRONTEND"
        public IActionResult PageNotFound()
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
        #endregion
    }
}
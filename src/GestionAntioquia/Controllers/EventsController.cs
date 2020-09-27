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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _evantService.GetAll());
        }

        // GET: Evant/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _event = await _evantService.Details(id);

            var _modelo = new EventView
            {
                EventId = _event.EventId,
                Name = _event.Name,
                BlogUrl = _event.EventUrl,
                Description = _event.Description,
                Author = _event.Author,
                CoverPage = _event.CoverPage,
                SquareCover = _event.SquareCover,
                EventDate = _event.EventsDate.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
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

        // GET: Event/Create
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateDto model)
        {
            if (ModelState.IsValid)
            {
                var _urlName = _evantService.DuplicaName(model.Name);

                if (_urlName)
                {
                    ViewData["DuplicaName"] = $"El Nombre {model.Name} ya ha sido utilizado, cambielo";
                    return View(model);
                }

                await _evantService.Create(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Event/Edit/5
        [Authorize(Roles = "Admin")]
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
                EventsDate = _event.EventsDate,
                Author = _event.Author,
                UpdateDate = _event.UpdateDate,
                State = _event.State
            };

            ViewData["CoverPage"] = _event.CoverPage;
            return View(_blog);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        [Route("/Eventos/")]
        public async Task<IActionResult> Event(
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
             .Where(x => x.State == true && x.Category.Name == "Event")
                         select q;

            int pageSize = 9;
            return View(await PaginatedList<Event>.CreateAsync(_query.AsNoTracking(), pag ?? 1, pageSize));

            //var _blog = _blogService.Blog();

            //   return View(await _blog);
        }

        // GET: Blogs/Details/5
        [Route("/eventos/{name}")]
        public async Task<IActionResult> Details(string name)
        {
            if (name == null)
            {
                return NotFound();
            }

            var _detail = await _evantService.Details(name);

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


            var _modelo = new EventView
            {
                EventId = _detail.EventId,
                Name = _detail.Name,
                BlogUrl = _detail.EventUrl,
                Description = _detail.Description,
                Author = _detail.Author,
                CoverPage = _detail.CoverPage,
                SquareCover = _detail.SquareCover,
                EventDate = _detail.EventsDate.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
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
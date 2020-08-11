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
    public class TouristExcursionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenericServicio _genericServicio;
        private readonly ICategoryService _categoryService;
        private readonly IPlaceService _placeService;
        private readonly ITouristExcursionsService _touristExcursionsService;


        public TouristExcursionsController(
                ApplicationDbContext context,
                IGenericServicio genericServicio,
                ITouristExcursionsService touristExcursionsService,
                ICategoryService categoryService,
                IPlaceService placeService)
        {
            _context = context;
            _touristExcursionsService = touristExcursionsService;
            _genericServicio = genericServicio;
            _categoryService = categoryService;
            _placeService = placeService;
        }

        #region "BACKEND"
        // GET: Tour
        public async Task<IActionResult> Index()
        {
            return View(await _touristExcursionsService.GetAll());
        }

        // GET: Tour/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _touristExcursionsService.Details(id);
            if (place == null)
            {
                return NotFound();
            }

            return View(place);
        }

        // GET: Tour/Create
        public IActionResult Create()
        {
            ViewData["TurismBusiness"] = new SelectList(_context
                                    .Places
                                    .Where(x => x.State == true
                                    && x.Category.Name == "tour"), "PlaceId", "Name");

            return View();
        }

        // POST: Tour/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TouristExcursionsCreateDto model)
        {

            if (ModelState.IsValid)
            {
                //var urlName = _context.Places
                //    .Where(x=>x.Name == model.Name);

                var _urlName = _touristExcursionsService.DuplicaName(model.Name);

                if (_urlName)
                {
                    ViewData["DuplicaName"] = $"El Nombre {model.Name} ya ha sido utilizado, cambielo";
                    ViewData["TurismBusiness"] = new SelectList(_context
                                            .Places
                                            .Where(x => x.State == true
                                            && x.Category.Name == "tour"), "PlaceId", "Name");

                    return View(model);
                }
                await _touristExcursionsService.Create(model);
                return RedirectToAction(nameof(Index));
            }

            ViewData["TurismBusiness"] = new SelectList(_context
                                    .Places
                                    .Where(x => x.State == true
                                    && x.Category.Name == "tour"), "PlaceId", "Name");
            return View(model);
        }


        // GET: Places/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }

            var _tour = await _touristExcursionsService.GetById(id);

            if (_tour == null)
            {
                return NotFound();
            }

            var _touristExcursionsEditDto = new TouristExcursionsEditDto
            {
                ProductId = _tour.ProductId,
                Name = _tour.Name,
                ProductUrl = _tour.ProductUrl,
                Description = _tour.Description,
                Status = _tour.Status,
                CategoryId = _tour.CategoryId
            };

            ViewData["TurismBusiness"] = new SelectList(_context
                                    .Places
                                    .Where(x => x.State == true
                                    && x.Category.Name == "tour"), "PlaceId", "Name");

            ViewData["CoverPage"] = _tour.CoverPage.ToString();
            return View(_touristExcursionsEditDto);
        }

        // POST: tour/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TouristExcursionsEditDto model)
        {

            if (id != model.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _touristExcursionsService.Edit(id, model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(model.PlaceId))
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
            ViewData["TurismBusiness"] = new SelectList(_context
                                    .Places
                                    .Where(x => x.State == true
                                    && x.Category.Name == "tour"), "PlaceId", "Name");

            return View(model);
        }

        // GET: tour/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _tour = await _touristExcursionsService.GetById(id);

            if (_tour == null)
            {
                return NotFound();
            }

            return View(_tour);
        }

        // POST: tour/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var _tour = await _touristExcursionsService.GetById(id);

            var _id = _tour.ProductId;
            var _cove = _tour.CoverPage;
            var _squareCover = _tour.SquareCover;

            await _touristExcursionsService.DeleteConfirmed(_id, _cove, _squareCover);

            //var product = await _context.Products.FindAsync(id);
            //_context.Products.Remove(product);
            //await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool ProductExists(int id)
        {
            return _touristExcursionsService.ProductExists(id);
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
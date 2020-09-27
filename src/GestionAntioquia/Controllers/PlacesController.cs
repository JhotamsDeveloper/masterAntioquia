using GestionAntioquia.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model.DTOs;
using Persisten.Database;
using System.Linq;
using Service;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace GestionAntioquia.Controllers
{
    public class PlacesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPlaceService _placeService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PlacesController(ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            IPlaceService placeService)
        {
            _context = context;
            _placeService = placeService;
            _webHostEnvironment = webHostEnvironment;
        }

        #region "BackEnd"

        // GET: Places
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _placeService.GetAll());
        }

        // GET: Places/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _placeService.Details(id);
            if (place == null)
            {
                return NotFound();
            }

            return View(place);
        }

        // GET: Places/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "Name");
            return View();
        }

        // POST: Places/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlaceCreateDto model)
        {

            if (ModelState.IsValid)
            {
                //var urlName = _context.Places
                //    .Where(x=>x.Name == model.Name);

                var _urlName = _placeService.DuplicaName(model.Name);

                if (_urlName)
                {
                    ViewData["DuplicaName"] = $"El Nombre {model.Name} ya ha sido utilizado, cambielo";
                    ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "Name", model.CategoryId);
                    return View(model);
                }
                await _placeService.Create(model);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "Name", model.CategoryId);
            return View(model);
        }

        // GET: Places/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }

            var place = await _placeService.GetById(id);
            //var place = await _placeService.GetByIdEdit(id);

            if (place == null)
            {
                return NotFound();
            }

            var _placeEditDto = new PlaceEditDto
            {
                PlaceId = place.PlaceId,
                Nit = place.Nit,
                Name = place.Name,
                Phone = place.Phone,
                Admin = place.Admin,
                City = place.City,
                urban = place.urban,
                Email = place.Email,
                Address = place.Address,
                Description = place.Description,
                Contract = place.Contract,
                State = place.State,
                CategoryId = place.CategoryId
            };

            ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "Name", place.CategoryId);
            ViewData["Logo"] = place.Logo.ToString();
            ViewData["CoverPage"] = place.CoverPage.ToString();
            return View(_placeEditDto);
        }

        // POST: Places/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlaceEditDto model)
        {

            if (id != model.PlaceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _placeService.Edit(id, model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaceExists(model.PlaceId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "Name", model.CategoryId);
            return View(model);
        }

        // GET: Places/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _placeService.GetById(id);
            //var place = await _context.Places
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(x => x.PlaceId == id);

            if (place == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Eliminar falló. Inténtalo de nuevo y si el problema persiste " +
                    "consulte al administrador del sistema";
            }

            return View(place);
        }

        // POST: Places/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var _place = await _placeService.GetById(id);

            var _id = _place.PlaceId;
            var _cover = _place.CoverPage;
            var _logo = _place.Logo;
            var _squareCover = _place.SquareCover;


            if (_place == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _placeService.DeleteConfirmed(_id, _cover, _logo, _squareCover);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }

        }

        private bool PlaceExists(int id)
        {
            return _placeService.CategoryExists(id);
        }

        #endregion

        #region "FrontEnd"
        public async Task<IActionResult> Restaurant()
        {
            var _restaurant = await _placeService.Restaurant();

            var _nuevo = DateTime.Now.AddMonths(1);

            var _viewAliados = (from a in _restaurant
                                select new PlacesRestaurantDto
                                {
                                    PlaceId = a.PlaceId,
                                    Name = a.Name,
                                    UrlName = a.NameUrl,
                                    Contract = a.Contract,
                                    SquareCover = a.SquareCover,
                                    Description = a.Description.Substring(0, 20),
                                    DataCreate = DateTime.Parse(a.CreationDate).AddMonths(1),
                                    New = "nuevo"
                                });

            return View(_viewAliados);

        }   
        #endregion



    }
}

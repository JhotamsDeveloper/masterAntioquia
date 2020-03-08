﻿using GestionAntioquia.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model.DTOs;
using Persisten.Database;
using Service;
using System;
using System.IO;
using System.Threading.Tasks;

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

        // GET: Places
        public async Task<IActionResult> Index()
        {
            return View(await _placeService.GetAll());
        }

        // GET: Places/Details/5
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
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "Icono");
            return View();
        }

        // POST: Places/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlaceCreateDto model)
        {
            if (ModelState.IsValid)
            {
                await _placeService.Create(model);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "Icono", model.CategoryId);
            return View(model);
        }

        // GET: Places/Edit/5
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

            var placeEditDto = new PlaceEditDto
            {
                PlaceId = place.PlaceId,
                Nit = place.Nit,
                Name = place.Name,
                Phone = place.Phone,
                Admin = place.Admin,
                Address = place.Address,
                Description = place.Description,
                Contract = place.Contract,
                State = place.State,
                CategoryId = place.CategoryId
            };

            ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "Icono", place.CategoryId);
            ViewData["Logo"] = place.Logo.ToString();
            ViewData["CoverPage"] = place.CoverPage.ToString();
            return View(placeEditDto);
        }

        // POST: Places/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "Icono", model.CategoryId);
            return View(model);
        }

        // GET: Places/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _placeService.GetById(id);
            if (place == null)
            {
                return NotFound();
            }

            return View(place);
        }

        // POST: Places/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var place = await _placeService.GetById(id);
            await _placeService.DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
        }

        private bool PlaceExists(int id)
        {
            return _placeService.CategoryExists(id);
        }
    }
}

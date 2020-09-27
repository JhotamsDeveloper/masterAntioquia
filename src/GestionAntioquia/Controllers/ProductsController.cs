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
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;
        private readonly IGalleryService _galleryService;
        public ProductsController(ApplicationDbContext context,
            IProductService productService,
            IGalleryService galleryService)
        {
            _context = context;
            _productService = productService;
            _galleryService = galleryService;
        }

        #region "BackEnd"
        // GET: Products
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetAll());

            //var applicationDbContext = _context.Products.Include(p => p.Place);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _product = await _productService.Details(id);

            //var product = await _context.Products
            //    .Include(p => p.Place)
            //    .FirstOrDefaultAsync(m => m.ProductId == id);

            if (_product == null)
            {
                return NotFound();
            }

            return View(_product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["Places"] = new SelectList(_context.Places
                                                .Include(x=>x.Category)
                                                .Where(x=>x.State == true && x.Category.Name == "souvenir"), "PlaceId", "Name");
            return View();
        }

        // POST: Products/Create
        // Para protegerse de ataques de superposición, habilite las propiedades específicas a las que desea enlazar, para
        // más detalles ver http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateDto model)
        {
            if (ModelState.IsValid)
            {

                var _duplicaName = _productService.DuplicaName(model.Name);

                if (_duplicaName)
                {
                    ViewData["DuplicaName"] = $"El Nombre {model.Name} ya ha sido utilizado, cambielo";
                    ViewData["Places"] = new SelectList(_context.Places
                                                        .Include(x => x.Category)
                                                        .Where(x => x.State == true && x.Category.Name == "souvenir"), "PlaceId", "Name", model.PlaceId);
                    return View(model);

                }
                await _productService.Create(model);

                //_context.Add(product);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Places"] = new SelectList(_context.Places
                                                .Include(x => x.Category)
                                                .Where(x => x.State == true && x.Category.Name == "souvenir"), "PlaceId", "Name", model.PlaceId);
            return View(model);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _product = await _productService.GetById(id);

            //var product = await _context.Products.FindAsync(id)
            if (_product == null)
            {
                return NotFound();
            }

            NumberFormatInfo nfi = new CultureInfo("es-CO", false).NumberFormat;
            nfi = (NumberFormatInfo)nfi.Clone();
            nfi.CurrencySymbol = "$";
            //Price = string.Format(nfi, "{0:C0}", _product.Price),

            var _galleries = _galleryService.GetAll().Where(x => x.ProducId == id).ToList();
            var _productEditDto = new ProductEditDto
            {
                ProductId = _product.ProductId,
                Name = _product.Name,
                Description = _product.Description,
                Price = _product.Price,
                Increments = _product.Increments,
                Discounts = _product.Discounts,
                PersonNumber = _product.PersonNumber,
                Statud = _product.Statud,
                Galleries = _galleries,
                PlaceId = _product.PlaceId
            };

            ViewData["Places"] = new SelectList(_context.Places
                                                .Include(x => x.Category)
                                                .Where(x => x.State == true && x.Category.Name == "souvenir"), "PlaceId", "Name", _product.PlaceId);
            ViewData["CoverPage"] = _product.CoverPage;
            return View(_productEditDto);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditDto model)
        {
            if (id != model.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.Edit(id, model);
                    //_context.Update(product);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(model.ProductId))
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
            ViewData["Places"] = new SelectList(_context.Places
                                                .Include(x => x.Category)
                                                .Where(x => x.State == true && x.Category.Name == "souvenir"), "PlaceId", "Name", model.PlaceId);
            return View(model);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var _product = await _context.Products
            //    .Include(p => p.Place)
            //    .FirstOrDefaultAsync(m => m.ProductId == id);

            var _product = await _productService.GetById(id);

            if (_product == null)
            {
                return NotFound();
            }

            return View(_product);
        }

        // POST: Products/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var _product = await _productService.GetById(id);

            var _id = _product.ProductId;
            var _cove = _product.CoverPage;
            var _squareCover = _product.SquareCover;

            await _productService.DeleteConfirmed(_id, _cove, _squareCover);

            //var product = await _context.Products.FindAsync(id);
            //_context.Products.Remove(product);
            //await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _productService.ProductExists(id);
        }
        #endregion

        #region "FrontEnd"

        // GET: Products
        [Route("donde-dormir/{productUrl}")]
        public async Task<IActionResult> Product(string productUrl)
        {
            if (productUrl == null)
            {
                return NotFound();
            }

            var _product = await _productService.ProductUrl(productUrl);

            NumberFormatInfo nfi = new CultureInfo("es-CO", false).NumberFormat;
            nfi = (NumberFormatInfo)nfi.Clone();
            nfi.CurrencySymbol = "$";

            var _priceWhitIncrement = _product.Increments + _product.Price;
            var _productWithDiscounts = _priceWhitIncrement - (_priceWhitIncrement * _product.Discounts / 100);

            string _urban = "";

            if (_product.Place.urban){_urban = "Urbano";}else{_urban = "Rural";}

            var _model = new ProductDetailView
            {
                Name = _product.Name,
                CoverPage = _product.CoverPage,
                SquareCover = _product.SquareCover,
                Description = _product.Description,
                PriceWhitIncrement = string.Format(nfi, "{0:C0}", _priceWhitIncrement),
                ProductWithDiscounts = string.Format(nfi, "{0:C0}", _productWithDiscounts),
                Discounts = _product.Discounts.ToString(),
                Statud = _product.Statud,
                PersonNumber = _product.PersonNumber,
                Place = _product.Place,
                Urban = _urban,
                Galleries = _product.Galleries
            };

            if (_model == null)
            {
                return NotFound();
            }

            return View(_model);

        }

        //GET: WhereToSleep
        [Route("donde-dormir")]
        public async Task<IActionResult> WhereToSleep()
        {

            var _whereToSleep = await _productService.WhereToSleep();

            NumberFormatInfo nfi = new CultureInfo("es-CO", false).NumberFormat;
            nfi = (NumberFormatInfo)nfi.Clone();
            nfi.CurrencySymbol = "$";


            var _whereToSleepView = (from a in _whereToSleep
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
                                         ProductWithDiscounts = string.Format(nfi, "{0:C0}",(a.Price + a.Increments) - ((a.Price + a.Increments) * a.Discounts / 100)),
                                         Statud = a.Statud,
                                         PersonNumber = a.PersonNumber,
                                         Place = a.Place
                                         
                                     });

            return View(_whereToSleepView);

        }

        #endregion
    }
}

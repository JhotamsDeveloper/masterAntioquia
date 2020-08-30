using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class StoreController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IStoreService _storeService;
        private readonly IGalleryService _galleryService;

        public StoreController(ApplicationDbContext context,
            IStoreService storeService,
            IGalleryService galleryService)
        {
            _context = context;
            _storeService = storeService;
            _galleryService = galleryService;
        }

        #region "BackEnd"

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _storeService.GetAll());

        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _product = await _storeService.Details(id);

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
        public IActionResult Create()
        {
            ViewData["Places"] = new SelectList(  _context
                                                .Places
                                                .Where(x=>x.State == true
                                                && x.Category.Name == "Tienda"), "PlaceId", "Name");
            return View();
        }

        // POST: Products/Create
        // Para protegerse de ataques de superposición, habilite las propiedades específicas a las que desea enlazar, para
        // más detalles ver http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StoreCreateDto model)
        {
            if (ModelState.IsValid)
            {
                var _duplicaName = _storeService.DuplicaName(model.Name);

                if (_duplicaName)
                {
                    ViewData["DuplicaName"] = $"El Nombre {model.Name} ya ha sido utilizado, cambielo";
                    ViewData["Places"] = new SelectList(_context.Places
                                                        .Include(x => x.Category)
                                                        .Where(x => x.State == true && x.Category.Name == "Tienda"), "PlaceId", "Name", model.PlaceId);
                    return View(model);

                }

                await _storeService.Create(model);
                return RedirectToAction(nameof(Index));
            }
            ViewData["Places"] = new SelectList(_context.Places
                                                .Include(x => x.Category)
                                                .Where(x => x.State == true && x.Category.Name == "Tienda"), "PlaceId", "Name", model.PlaceId);
            return View(model);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _storeProdStore = await _storeService.GetById(id);

            //var product = await _context.Products.FindAsync(id)
            if (_storeProdStore == null)
            {
                return NotFound();
            }

            var _galleries = _galleryService.GetAll().Where(x => x.ProducId == id).ToList();

            NumberFormatInfo nfi = new CultureInfo("es-CO", false).NumberFormat;
            nfi = (NumberFormatInfo)nfi.Clone();
            nfi.CurrencySymbol = "$";
            //Price = string.Format(nfi, "{0:C0}", _storeProdStore.Price),

            var _producStoreEditDto = new StoreEditDto
            {
                ProductId = _storeProdStore.ProductId,
                Name = _storeProdStore.Name,
                Description = _storeProdStore.Description,
                Mineral = _storeProdStore.Mineral,
                Price = _storeProdStore.Price,
                ShippingValue = _storeProdStore.ShippingValue,
                Discounts = _storeProdStore.Discounts,
                Increments = _storeProdStore.Increments,
                Statud = _storeProdStore.Statud,
                AmountSupported = _storeProdStore.AmountSupported,
                Galleries = _galleries,
                PlaceId = _storeProdStore.PlaceId
            };

            ViewData["Places"] = new SelectList(_context
                                    .Places
                                    .Where(x => x.State == true
                                    && x.Category.Name == "Tienda"), "PlaceId", "Name");

            ViewData["CoverPage"] = _storeProdStore.CoverPage;
            return View(_producStoreEditDto);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StoreEditDto model)
        {
            if (id != model.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _storeService.Edit(id, model);
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

            ViewData["Places"] = new SelectList(_context
                                    .Places
                                    .Where(x => x.State == true
                                    && x.Category.Name == "Tienda"), "PlaceId", "Name");
            return View(model);
        }

        private bool ProductExists(int id)
        {
            return _storeService.ProductExists(id);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var _product = await _context.Products
            //    .Include(p => p.Place)
            //    .FirstOrDefaultAsync(m => m.ProductId == id);

            var _storeProduct = await _storeService.GetById(id);

            if (_storeProduct == null)
            {
                return NotFound();
            }

            return View(_storeProduct);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var _storeProduct = await _storeService.GetById(id);

            var _id = _storeProduct.ProductId;
            var _cove = _storeProduct.CoverPage;
            var _squareCover = _storeProduct.SquareCover;

            await _storeService.DeleteConfirmed(_id, _cove, _squareCover);

            //var product = await _context.Products.FindAsync(id);
            //_context.Products.Remove(product);
            //await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        #endregion

        //GET: Store Products
        [Route("tienda-virtual")]
        public async Task<IActionResult> StoreProducts()
        {

            var _products = await _storeService.StoreProducts();

            NumberFormatInfo nfi = new CultureInfo("es-CO", false).NumberFormat;
            nfi = (NumberFormatInfo)nfi.Clone();
            nfi.CurrencySymbol = "$";

            var _filigreeView = (from a in _products
                                 select new StoreView
                                 {
                                     ProductId = a.ProductId,
                                     Name = a.Name,
                                     ProductUrl = a.ProductUrl,
                                     SquareCover = a.SquareCover,
                                     Description = a.Description,
                                     Mineral = a.Mineral,
                                     PriceWhitIncrement = string.Format(nfi, "{0:C0}", a.Price + a.Increments),
                                     ProductWithDiscounts = string.Format(nfi, "{0:C0}", (a.Price + a.Increments) - ((a.Price + a.Increments) * a.Discounts / 100)),
                                     ShippingValue = string.Format(nfi, "{0:C0}", a.ShippingValue),
                                     Discounts = a.Discounts,
                                     Statud = a.Statud,
                                     Place = a.Place
                                 });

            return View(_filigreeView);

        }

        // GET: Products
        [Route("tienda-virtual/{urlProductStore}")]
        public async Task<IActionResult> ShopProducts(string urlProductStore)
        {
            if (urlProductStore == null)
            {
                return NotFound();
            }

            var _product = await _storeService.ProductUrl(urlProductStore);

            NumberFormatInfo nfi = new CultureInfo("es-CO", false).NumberFormat;
            nfi = (NumberFormatInfo)nfi.Clone();
            nfi.CurrencySymbol = "$";

            var _priceWhitIncrement = _product.Increments + _product.Price;
            var _productWithDiscounts = _priceWhitIncrement - (_priceWhitIncrement * _product.Discounts / 100);

            string _urban = "";

            if (_product.Place.urban) { _urban = "Urbano"; } else { _urban = "Rural"; }

            var _model = new StoreDetailView
            {
                Name = _product.Name,
                CoverPage = _product.CoverPage,
                Description = _product.Description,
                Mineral = _product.Mineral,
                PriceWhitIncrement = string.Format(nfi, "{0:C0}", _priceWhitIncrement),
                ProductWithDiscounts = string.Format(nfi, "{0:C0}", _productWithDiscounts),
                Discounts = _product.Discounts.ToString(),
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
    }
}
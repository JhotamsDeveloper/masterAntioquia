using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetAll());

            //var applicationDbContext = _context.Products.Include(p => p.Place);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
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
        public IActionResult Create()
        {
            ViewData["Name"] = new SelectList(_context.Places, "PlaceId", "Name");
            return View();
        }

        // POST: Products/Create
        // Para protegerse de ataques de superposición, habilite las propiedades específicas a las que desea enlazar, para
        // más detalles ver http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateDto model)
        {
            if (ModelState.IsValid)
            {
                await _productService.Create(model);

                //_context.Add(product);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlaceId"] = new SelectList(_context.Places, "PlaceId", "Name", model.PlaceId);
            return View(model);
        }

        // GET: Products/Edit/5
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

            var _galleries = _galleryService.GetAll().Where(x => x.ProducId == id).ToList();
            var _productEditDto = new ProductEditDto
            {
                ProductId = _product.ProductId,
                Name = _product.Name,
                Description = _product.Description,
                Price = string.Format(nfi, "{0:C0}", _product.Price),
                Discounts = _product.Discounts,
                PersonNumber = _product.PersonNumber,
                Statud = _product.Statud,
                Galleries = _galleries,
                PlaceId = _product.PlaceId
            };

            ViewData["PlaceId"] = new SelectList(_context.Places, "PlaceId", "Name", _product.PlaceId);
            ViewData["CoverPage"] = _product.CoverPage;
            return View(_productEditDto);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["PlaceId"] = new SelectList(_context.Places, "PlaceId", "Name", model.PlaceId);
            return View(model);
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

            var _product = await _productService.GetById(id);

            if (_product == null)
            {
                return NotFound();
            }

            return View(_product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var _product = await _productService.GetById(id);

            var _id = _product.ProductId;
            var _cove = _product.CoverPage;

            await _productService.DeleteConfirmed(_id, _cove);

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
        public async Task<IActionResult> Product(string productUrl)
        {
            if (productUrl == null)
            {
                return NotFound();
            }

            var _detalleHotel = await _productService.ProductUrl(productUrl);

            if (_detalleHotel == null)
            {
                return NotFound();
            }

            return View(_detalleHotel);

        }

        //GET: WhereToSleep
        public async Task<IActionResult> WhereToSleep()
        {

            var _whereToSleep = await _productService.WhereToSleep();

            NumberFormatInfo nfi = new CultureInfo("es-CO", false).NumberFormat;
            nfi = (NumberFormatInfo)nfi.Clone();
            nfi.CurrencySymbol = "$";

            var _whereToSleepView = (from a in _whereToSleep
                                     select new ProductViewDto {

                                         ProductId = a.ProductId,
                                         Name = a.Name,
                                         ProductUrl = a.ProductUrl,
                                         CoverPage = a.CoverPage,
                                         SquareCover = a.SquareCover,
                                         Description = a.Description,
                                         Price = string.Format(nfi, "{0:C0}", a.Price),
                                         Discounts = a.Discounts,
                                         Statud = a.Statud,
                                         PersonNumber = a.PersonNumber,
                                         Place = a.Place

                                     });

            return View(_whereToSleepView);

        }

        #endregion
    }
}

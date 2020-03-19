using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model;
using Persisten.Database;
using Service;

namespace GestionAntioquia.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;

        public ProductsController(ApplicationDbContext context,
            IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

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
            ViewData["PlaceId"] = new SelectList(_context.Places, "PlaceId", "PlaceId");
            return View();
        }

        // POST: Products/Create
        // Para protegerse de ataques de superposición, habilite las propiedades específicas a las que desea enlazar, para
        // más detalles ver http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,CoverPage,Description,Price,HighPrice,HalfPrice,LowPrice,Discounts,Statud,CreationDate,UpdateDate,PlaceId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlaceId"] = new SelectList(_context.Places, "PlaceId", "PlaceId", product.PlaceId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["PlaceId"] = new SelectList(_context.Places, "PlaceId", "PlaceId", product.PlaceId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,CoverPage,Description,Price,HighPrice,HalfPrice,LowPrice,Discounts,Statud,CreationDate,UpdateDate,PlaceId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["PlaceId"] = new SelectList(_context.Places, "PlaceId", "PlaceId", product.PlaceId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Place)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}

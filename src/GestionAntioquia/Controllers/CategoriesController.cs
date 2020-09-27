using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.DTOs;
using Persisten.Database;
using Service;
using System.Threading.Tasks;

namespace GestionAntioquia.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly ICategoryService _categoryService;

        public CategoriesController(ApplicationDbContext context,
            ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var category = await _categoryService.GetAll();
              
            return View(category);
            //return View(await _context.Categorys.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryService.Details(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // Para protegerse de los ataques de sobreposición, habilite las propiedades específicas a las que desea unirse, para
        // más detalles ver http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.Create(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        //// GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Edit/5
        // Para protegerse de ataques de superposición, habilite las propiedades específicas a las que desea enlazar, para
        // más detalles ver http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryEditDto model) 
        {
            if (id != model.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.Edit(id, model);
                }
                catch (DbUpdateConcurrencyException)
                {

                    if (!CategoryExists(model.CategoryId))
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

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryService.DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _categoryService.CategoryExists(id);
        }

        public IActionResult Inline()
        {
            ViewBag.items = new[] { "Bold", "Italic", "Underline",
                "Formats", "-", "Alignments", "OrderedList", "UnorderedList",
                "CreateLink" };
            return View();
        }
    }
}

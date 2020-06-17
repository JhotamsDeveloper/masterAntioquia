using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs;
using Persisten.Database;
using Service;

namespace GestionAntioquia.Controllers
{
    public class StoreController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IStoreService _storeService;
        public StoreController(ApplicationDbContext context,
            IStoreService storeService)
        {
            _context = context;
            _storeService = storeService;
        }

        #region "BackEnd"

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _storeService.GetAll());

        }

        #endregion

        //GET: Filigree
        public async Task<IActionResult> StoreProducts()
        {

            var _products = await _storeService.StoreProducts();

            NumberFormatInfo nfi = new CultureInfo("es-CO", false).NumberFormat;
            nfi = (NumberFormatInfo)nfi.Clone();
            nfi.CurrencySymbol = "$";

            var _filigreeView = (from a in _products
                                 select new StoreProductsDto
                                 {

                                     ProductId = a.ProductId,
                                     Name = a.Name,
                                     ProductUrl = a.ProductUrl,
                                     CoverPage = a.CoverPage,
                                     SquareCover = a.SquareCover,
                                     Description = a.Description,
                                     Price = string.Format(nfi, "{0:C0}", a.Price),
                                     Discounts = a.Discounts,
                                     Statud = a.Statud,
                                     Place = a.Place

                                 });

            return View(_filigreeView);

        }
    }
}
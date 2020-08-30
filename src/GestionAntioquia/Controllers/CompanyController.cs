using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GestionAntioquia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs;
using Persisten.Database;
using Service;

namespace GestionAntioquia.Controllers
{
    public class CompanyController : Controller
    {

        private readonly ICompanyService _companyService;

        [TempData]
        public string _StatusMessaje { get; set; }

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        // GET: hotel

        [Route("souvenir")]
        public async Task<IActionResult> Allies()
        {
            var _aliados = await _companyService.GetAll();

            var _viewAliados = from a in _aliados
                                select new PlacesAlliesDto
                                {
                                    PlaceId = a.PlaceId,
                                    Name = a.Name,
                                    City = a.City,
                                    urban = a.urban,
                                    UrlName = a.NameUrl,
                                    Contract = a.Contract,
                                    SquareCover = a.SquareCover,
                                    Description = a.Description.Substring(0, 100),
                                    DataCreate = DateTime.Parse(a.CreationDate).AddMonths(1),
                                    New = "nuevo",
                                    NameCategory = a.Category.Name
                                };

            return View(_viewAliados);
        }

        // GET: hotel/Details/5
        public async Task<ActionResult> Details(string urlName)
        {
            try
            {

                //Variables
                string _urbano ="";

                if (urlName == null)
                {
                    return NotFound();
                }

                var _detaislPlace = await _companyService.Details(urlName);

                var _products = await _companyService.DetailProducts(_detaislPlace.PlaceId);
                var _review = await _companyService.ReviewsPlaces(_detaislPlace.PlaceId);
                var _totalReviews = _companyService.TotalReviews(_detaislPlace.PlaceId);

                NumberFormatInfo nfi = new CultureInfo("es-CO", false).NumberFormat;
                nfi = (NumberFormatInfo)nfi.Clone();
                nfi.CurrencySymbol = "$";

                var _productView = from p in _products
                                   select new ProductPlacesView
                                   {
                                       ProductUrl = p.ProductUrl,
                                       CoverPage = p.CoverPage,
                                       Name = p.Name,
                                       Description = p.Description,
                                       NumberOfPeople = p.PersonNumber,
                                       PriceWhitIncrement = string.Format(nfi, "{0:C0}", (p.Price + p.Increments)),
                                       Discounts = p.Discounts,
                                       ProductWithDiscounts = string.Format(nfi, "{0:C0}", (p.Price + p.Increments) - ((p.Price + p.Increments) * p.Discounts / 100)),
                                   };

                var _reviews = from r in _review
                               select new ReviewsGetView
                               {
                                   TittleReview = r.TittleReview,
                                   Description = r.Description,
                                   Assessment = r.Assessment,
                                   NameUser = r.UserName,
                                   DateCreateReview = r.ReviewCreateDate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                                   Galleries = r.Galleries.ToList(),
                               };

                if (_detaislPlace.urban) { _urbano = "Urbana"; }
                else
                {
                    _urbano = "rural";
                };

                var _placesDetailsView = new PlacesDetailsView
                {
                    PlaceId = _detaislPlace.PlaceId,
                    Nit = _detaislPlace.Nit,
                    Name = _detaislPlace.Name,
                    Phone = _detaislPlace.Phone,
                    Admin = _detaislPlace.Address,
                    Email = _detaislPlace.Email,
                    Address = _detaislPlace.Address,
                    City = _detaislPlace.City,
                    urban = _urbano,
                    Description = _detaislPlace.Description,
                    NameUrl = _detaislPlace.NameUrl,
                    CoverPage = _detaislPlace.CoverPage,
                    Logo = _detaislPlace.Logo,
                    Contract = _detaislPlace.Contract,
                    CreationDate = _detaislPlace.CreationDate,
                    stateMessageCreate = _StatusMessaje,
                    TotalReviews = Convert.ToInt16(_totalReviews),
                    LatitudeCoordinates = null,
                    LengthCoordinates = null,
                    Products = _productView.ToList(),
                    Reviews = _reviews.ToList(),
                };

                if (_StatusMessaje != null)
                {
                    ViewData["successful"] = _StatusMessaje;
                }

                if (_placesDetailsView == null)
                {
                    return NotFound();
                }

                return View(_placesDetailsView);
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        // GET: hotel/Details/5
        public async Task<ActionResult> Products(string urlName)
        {

            if (urlName == null)
            {
                return NotFound();
            }

            var _detalleHotel = await _companyService.Details(urlName);

            if (_detalleHotel == null)
            {
                return NotFound();
            }

            return View(_detalleHotel);
        }

        [Authorize(Roles = "UserApp, Admin")]
        public async Task<ActionResult> Reviews(ReviewsCreateDto model, string urlPlaceReview)
        {

            try
            {
                var _url = await _companyService.CreateReviews(model);
                _StatusMessaje = "show";
                return Redirect(url: "/souvenir/" + urlPlaceReview);
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
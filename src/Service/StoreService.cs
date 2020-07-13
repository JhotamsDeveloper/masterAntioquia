using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Model;
using Persisten.Database;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Service.Commons;
using Model.DTOs;
using System.Globalization;

namespace Service
{
    public interface IStoreService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<StoreDto> Details(int? id);
        Task<StoreDto> Create(StoreCreateDto model);
        Task<StoreDto> GetById(int? id);
        Task Edit(int id, StoreEditDto model);
        bool ProductExists(int id);
        Task DeleteConfirmed(int _id, string _cover);
        Task<IEnumerable<Product>> StoreProducts();
        Task<StoreDto> ProductUrl(string productUrl);
    }

    public class StoreService : IStoreService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUploadedFile _uploadedFile;
        private readonly IGalleryService _galleryService;
        private readonly IFormatString _formatString;


        public StoreService(
            ApplicationDbContext context,
            IMapper mapper,
            IUploadedFile uploadedFile,
            IGalleryService galleryService,
            IFormatString formatString)
        {
            _context = context;
            _mapper = mapper;
            _uploadedFile = uploadedFile;
            _galleryService = galleryService;
            _formatString = formatString;
        }

        #region "BackEnd"

        public async Task<IEnumerable<Product>> GetAll()
            {
                var _getAll = _context.Products
                .Include(p => p.Place)
                .ThenInclude(c=>c.Category)
                .Where(x=>x.Statud == true && x.Place.Category.Name == "Tienda");
                return (await _getAll.ToListAsync());
            }

        public async Task<StoreDto> Details(int? id)
        {

            var _products = _mapper.Map<StoreDto>(
                    await _context.Products
                    .Include(p => p.Place)
                    .FirstOrDefaultAsync(m => m.ProductId == id)
                );

            return _mapper.Map<StoreDto>(_products);
        }

        public async Task<StoreDto> Create(StoreCreateDto model)
        {

            using (var transaction = _context.Database.BeginTransaction())

            {
                try
                {

                    var _coverPage = _uploadedFile.UploadedFileImage(model.CoverPage);
                    var _SquareCover = _uploadedFile.UploadedFileImage(model.SquareCover);
                    var _fechaActual = DateTime.Now;
                    var _url = _formatString.FormatUrl(model.Name);

                    NumberFormatInfo nfi = new CultureInfo("es-CO", false).NumberFormat;
                    nfi = (NumberFormatInfo)nfi.Clone();
                    nfi.CurrencySymbol = "$";

                    var _product = new Product
                    {
                        Name = model.Name,
                        ProductUrl = _url,
                        CoverPage = _coverPage,
                        SquareCover = _SquareCover,
                        Description = model.Description,
                        Mineral = model.Mineral,
                        Price = string.Format(nfi, "{0:C0}", model.Price),
                        Increments = model.Increments,
                        Discounts = model.Discounts,
                        AmountSupported = model.AmountSupported,
                        ShippingValue = model.ShippingValue,
                        Statud = model.Statud,
                        PlaceId = model.PlaceId,

                        CreationDate = _fechaActual
                    };

                    await _context.AddAsync(_product);
                    await _context.SaveChangesAsync();

                    _mapper.Map<StoreDto>(_product);

                    List<string> _uploadGalleries = _uploadedFile.UploadedMultipleFileImage(model.Gallery);

                    for (int i = 0; i < _uploadGalleries.Count; i++)
                    {
                        var _gallery = new Gallery
                        {
                            ProducId = _product.ProductId,
                            NameImage = _uploadGalleries[i]
                        };

                        await _context.AddAsync(_gallery);
                        await _context.SaveChangesAsync();
                        _mapper.Map<GalleryDto>(_gallery);

                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }

            return null;

        }

        public async Task<StoreDto> GetById(int? id)
        {

            return _mapper.Map<StoreDto>(
                await _context.Products
                .AsNoTracking()
                .Include(g => g.Galleries)
                .FirstOrDefaultAsync(x => x.ProductId == id)
                );

        }

        public async Task Edit(int id, StoreEditDto model)
        {
            using (var transaction = _context.Database.BeginTransaction())

            {
                try
                {
                    DateTime _dateUpdate = DateTime.Now;
                    var _producStoreEditDto = await _context.Products.SingleAsync(x => x.ProductId == id);
                    var _coverPage = _uploadedFile.UploadedFileImage(_producStoreEditDto.CoverPage, model.CoverPage);
                    var _squareCover = _uploadedFile.UploadedFileImage(_producStoreEditDto.SquareCover, model.SquareCover);

                    NumberFormatInfo nfi = new CultureInfo("es-CO", false).NumberFormat;
                    nfi = (NumberFormatInfo)nfi.Clone();
                    nfi.CurrencySymbol = "$";

                    if (_coverPage == null)
                    {
                        _coverPage = _producStoreEditDto.CoverPage;
                    }

                    _producStoreEditDto.Name = model.Name;
                    _producStoreEditDto.CoverPage = _coverPage;
                    _producStoreEditDto.SquareCover = _squareCover;
                    _producStoreEditDto.Description = model.Description;
                    _producStoreEditDto.Mineral = model.Mineral;
                    _producStoreEditDto.Price = string.Format(nfi, "{0:C0}", model.Price);
                    _producStoreEditDto.ShippingValue = model.ShippingValue;
                    _producStoreEditDto.Discounts = model.Discounts;
                    _producStoreEditDto.Increments = model.Increments;
                    _producStoreEditDto.Statud = model.Statud;
                    _producStoreEditDto.AmountSupported = model.AmountSupported;
                    _producStoreEditDto.PlaceId = model.PlaceId;
                    _producStoreEditDto.UpdateDate = DateTime.Now;

                    await _context.SaveChangesAsync();

                    if (model.Gallery != null)
                    {

                        var _getGalleries = _galleryService.GetAll().Where(x => x.ProducId == id).ToList();
                        var _idsGalleries = _getGalleries.Select(x => x.GalleryId).ToList();

                        if (_idsGalleries.Count > 0)
                        {
                            foreach (var item in _idsGalleries)
                            {

                                _context.Remove(new Gallery
                                {
                                    GalleryId = item
                                });
                                await _context.SaveChangesAsync();

                            }
                        }

                        var _galleries = _uploadedFile.UploadedMultipleFileImage(model.Gallery, _getGalleries.Select(x => x.NameImage).ToList());

                        for (int i = 0; i < _galleries.Count; i++)
                        {
                            var _gallery = new Gallery
                            {
                                ProducId = _producStoreEditDto.ProductId,
                                NameImage = _galleries[i]
                            };

                            await _context.AddAsync(_gallery);
                            await _context.SaveChangesAsync();
                            _mapper.Map<GalleryDto>(_gallery);
                        }

                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }

        public bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        public async Task DeleteConfirmed(int _id, string _cover)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    if (_cover != null)
                    {
                        _uploadedFile.DeleteConfirmed(_cover);
                    }

                    var _getGalleries = _galleryService.GetAll().Where(x => x.ProducId == _id).ToList();
                    var _idsGalleries = _getGalleries.Select(x => x.GalleryId).ToList();
                    var _galleries = _uploadedFile.UploadedMultipleFileImage(_getGalleries.Select(x => x.NameImage).ToList());

                    
                    if (_idsGalleries.Count > 0)
                    {
                        foreach (var item in _idsGalleries)
                        {

                            _context.Remove(new Gallery
                            {
                                GalleryId = item

                            });
                            await _context.SaveChangesAsync();

                        }
                    }

                    _context.Remove(new Product
                    {
                        ProductId = _id

                    });
                    await _context.SaveChangesAsync();

                    //_context.Remove(new Place
                    //{
                    //    PlaceId = _id
                    //});

                    //await _context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }

            }
        }
        #endregion

        #region FrontEnd
        public async Task<IEnumerable<Product>> StoreProducts()
        {
            var _getAll = _context.Products
                .Include(p => p.Place)
                .ThenInclude(c => c.Category)
                .Where(x => x.Statud == true && x.Place.Category.Name == "Tienda");

            return (await _getAll.ToListAsync());
        }
        public async Task<StoreDto> ProductUrl(string productUrl)
        {
            var _productUrl = _mapper.Map<StoreDto>(
                    await _context.Products
                    .Include(p => p.Place)
                    .Include(g => g.Galleries)
                    .Where(s => s.Statud == true)
                    .FirstOrDefaultAsync(m => m.ProductUrl == productUrl)

                );

            return _mapper.Map<StoreDto>(_productUrl);
        }

        #endregion

    }
}

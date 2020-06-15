using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTOs;
using Persisten.Database;
using Service.Commons;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<ProductDto> Details(int? id);
        Task<ProductDto> Create(ProductCreateDto model);
        Task<ProductDto> GetById(int? id);
        Task Edit(int id, ProductEditDto model);
        Task DeleteConfirmed(int _id, string _cover);
        Task<ProductDto> ProductUrl(string productUrl);
        Task<IEnumerable<Product>> WhereToSleep();
        Task<IEnumerable<Product>> Filigree();
        bool ProductExists(int id);
    }

    public class ProductService : IProductService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IUploadedFile _uploadedFile;
        private readonly IGalleryService _galleryService;
        private readonly IMapper _mapper;

        public ProductService(ApplicationDbContext context,
            IUploadedFile uploadedFile,
            IGalleryService galleryService,
            IMapper mapper)
        {
            _context = context;
            _uploadedFile = uploadedFile;
            _galleryService = galleryService;
            _mapper = mapper;
        }


        public async Task<IEnumerable<Product>> GetAll()
        {
            var _getAll = _context.Products.Include(p => p.Place);
            return (await _getAll.ToListAsync());
        }

        public IEnumerable<Product> GetAllTest()
        {
            return _context.Products.ToList();
        }

        public async Task<ProductDto> Details(int? id)
        {

            var _products = _mapper.Map<ProductDto>(
                    await _context.Products
                    .Include(p => p.Place)
                    .FirstOrDefaultAsync(m => m.ProductId == id)
                );

            return _mapper.Map<ProductDto>(_products);
        }

        public async Task<ProductDto> Create(ProductCreateDto model)
        {

            using (var transaction = _context.Database.BeginTransaction())

            {
                try
                {

                    var _coverPage = _uploadedFile.UploadedFileImage(model.CoverPage);
                    var _fechaActual = DateTime.Now;
                    var _url = FormatString(model.Name);

                    var _product = new Product
                    {
                        Name = model.Name,
                        ProductUrl = _url,
                        CoverPage = _coverPage,
                        Description = model.Description,
                        Price = model.Price,
                        Discounts = model.Discounts,
                        AmountSupported = model.AmountSupported,
                        PersonNumber = model.PersonNumber,
                        Statud = model.Statud,
                        PlaceId = model.PlaceId,
                        
                        CreationDate = _fechaActual
                    };

                    await _context.AddAsync(_product);
                    await _context.SaveChangesAsync();

                    _mapper.Map<ProductDto>(_product);

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

        public async Task<ProductDto> GetById(int? id)
        {

            return _mapper.Map<ProductDto>(
                await _context.Products
                .AsNoTracking()
                .Include(g => g.Galleries)
                .FirstOrDefaultAsync(x => x.ProductId == id)
                );

        }

        public async Task Edit(int id, ProductEditDto model)
        {
            using (var transaction = _context.Database.BeginTransaction())

            {
                try
                {
                    DateTime _dateUpdate = DateTime.Now;
                    var _product = await _context.Products.SingleAsync(x => x.ProductId == id);
                    var _coverPage = _uploadedFile.UploadedFileImage(_product.CoverPage, model.CoverPage);
                    var _squareCover = _uploadedFile.UploadedFileImage(_product.SquareCover, model.SquareCover);

                    if (_coverPage == null)
                    {
                        _coverPage = _product.CoverPage;
                    }

                    _product.Name = model.Name;
                    _product.CoverPage = _coverPage;
                    _product.SquareCover = _squareCover;
                    _product.Description = model.Description;
                    _product.Price = model.Price;
                    _product.Discounts = model.Discounts;
                    _product.PersonNumber = model.PersonNumber;
                    _product.Statud = model.Statud;
                    _product.AmountSupported = model.AmountSupported;
                    _product.PlaceId = model.PlaceId;
                    _product.UpdateDate = DateTime.Now;

                    await _context.SaveChangesAsync();

                    if (model.Gallery != null)
                    {

                        var _getGalleries = _galleryService.GetAll().Where(x => x.ProducId == id).ToList();
                        var _idsGalleries = _getGalleries.Select(x => x.GalleryId).ToList();

                        if (_idsGalleries.Count >0)
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
                                ProducId = _product.ProductId,
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

                    _context.Remove(new Product
                    {
                        ProductId = _id

                    });
                    await _context.SaveChangesAsync();

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

        public async Task<ProductDto> ProductUrl(string productUrl)
        {
            var _productUrl = _mapper.Map<ProductDto>(
                    await _context.Products
                    .Include(p=>p.Place)
                    .Include(g=>g.Galleries)
                    .Where(s => s.Statud == true)
                    .FirstOrDefaultAsync(m => m.ProductUrl == productUrl)

                );

            return _mapper.Map<ProductDto>(_productUrl);
        }

        public bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        public async Task<IEnumerable<Product>> WhereToSleep()
        {
            var _getAll = _context.Products
                .Include(p => p.Place)
                .ThenInclude(c => c.Category)
                .Where(x => x.Statud == true && x.Place.Category.Name == "Hotel");

            return (await _getAll.ToListAsync());
        }

        public async Task<IEnumerable<Product>> Filigree()
        {
            var _getAll = _context.Products
                .Include(p => p.Place)
                .ThenInclude(c => c.Category)
                .Where(x => x.Statud == true && x.Place.Category.Name == "Filigrana");

            return (await _getAll.ToListAsync());
        }

        private async Task DeleteGalleries(int id)
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
        }

        private String FormatString(String texto)
        {
            var original = "ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýÿ ";
            // Cadena de caracteres ASCII que reemplazarán los originales.
            var ascii = "AAAAAAACEEEEIIIIDNOOOOOOUUUUYBaaaaaaaceeeeiiiionoooooouuuuyy-";
            var output = texto;
            for (int i = 0; i < original.Length; i++)
            {
                // Reemplazamos los caracteres especiales.

                output = output.Replace(original[i], ascii[i]);

            }

            return output;
        }
    }
}

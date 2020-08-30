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
using System.Globalization;

namespace Service
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<ProductDto> Details(int? id);
        Task<ProductDto> Create(ProductCreateDto model);
        Task<ProductDto> GetById(int? id);
        Task Edit(int id, ProductEditDto model);
        Task DeleteConfirmed(int _id, string _cover, string _squareCover);
        Task<ProductDto> ProductUrl(string productUrl);
        Task<IEnumerable<Product>> WhereToSleep();
        bool ProductExists(int id);
        bool DuplicaName(string name);
    }

    public class ProductService : IProductService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IGalleryService _galleryService;
        private readonly IFormatString _formatString;
        private readonly IUploadedFile _uploadedFile;

        //Variables para Azure
        private readonly IUploadedFileAzure _uploadedFileAzure;
        private readonly string _account = "products";

        public ProductService(
            ApplicationDbContext context,
            IMapper mapper,
            IGalleryService galleryService,
            IUploadedFile uploadedFile,
            IUploadedFileAzure uploadedFileAzure,
            IFormatString formatString)
        {
            _context = context;
            _mapper = mapper;
            _galleryService = galleryService;
            _uploadedFile = uploadedFile;
            _uploadedFileAzure = uploadedFileAzure;
            _formatString = formatString;
        }


        public async Task<IEnumerable<Product>> GetAll()
        {
            var _getAll = _context.Products
                .Include(p => p.Place)
                .Where(x=>x.Place.Category.Name == "souvenir");
            return (await _getAll.ToListAsync());
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

                    //var _coverPage = await _uploadedFileAzure.SaveFileAzure(model.CoverPage, _account);
                    //var _squareCover = await _uploadedFileAzure.SaveFileAzure(model.SquareCover, _account);

                    var _coverPage = _uploadedFile.UploadedFileImage(model.CoverPage);
                    var _squareCover = _uploadedFile.UploadedFileImage(model.SquareCover);
                    var _fechaActual = DateTime.Now;
                    var _url = _formatString.FormatUrl(model.Name);

                    var _product = new Product
                    {
                        Name = model.Name.Trim(),
                        ProductUrl = _url.ToLower(),
                        CoverPage = _coverPage,
                        SquareCover = _squareCover,
                        Description = model.Description,
                        Price = model.Price,
                        Increments = model.Increments,
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


                    if (model.Gallery.Any())
                    {
                        List<string> _uploadGalleries = _uploadedFile.UploadedMultipleFileImage(model.Gallery);
                        int _accountant = 0;

                        foreach (var item in model.Gallery)
                        {

                            var _gallery = new Gallery
                            {
                                ProducId = _product.ProductId,
                                NameImage = _uploadGalleries[_accountant]
                            };

                            await _context.AddAsync(_gallery);
                            await _context.SaveChangesAsync();
                            _mapper.Map<GalleryDto>(_gallery);

                            _accountant++;
                        }

                    }

                    //MÉTODO PARA AZURE

                    //if (model.Gallery.Any())
                    //{
                    //    string[] _uploadGalleries = new string[model.Gallery.Count()];
                    //    int _accountant = 0;

                    //    foreach (var item in model.Gallery)
                    //    {
                    //        _uploadGalleries[_accountant] = await _uploadedFileAzure.SaveFileAzure(item, _account);

                    //        var _gallery = new Gallery
                    //        {
                    //            ProducId = _product.ProductId,
                    //            NameImage = _uploadGalleries[_accountant]
                    //        };

                    //        await _context.AddAsync(_gallery);
                    //        await _context.SaveChangesAsync();
                    //        _mapper.Map<GalleryDto>(_gallery);

                    //        _accountant++;
                    //    }

                    //}


                    transaction.Commit();
                }
                catch (Exception ex)
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
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                DateTime _dateUpdate = DateTime.Now;
                var _product = await _context.Products.SingleAsync(x => x.ProductId == id);
                var _coverPage = "";
                var _squareCover = "";

                if (model.CoverPage != null)
                {
                    if (_product.CoverPage != null)
                    {
                        //await _uploadedFileAzure.DeleteFile(_product.CoverPage, _account);
                        _uploadedFile.DeleteConfirmed(_product.CoverPage);
                    }

                    //_coverPage = await _uploadedFileAzure.SaveFileAzure(model.CoverPage, _account);
                    _coverPage = _uploadedFile.UploadedFileImage(model.CoverPage);
                }
                else
                {
                    _coverPage = _product.CoverPage;
                }

                if (model.SquareCover != null)
                {
                    if (_product.SquareCover != null)
                    {
                        _uploadedFile.DeleteConfirmed(_product.SquareCover);
                        //await _uploadedFileAzure.DeleteFile(_product.SquareCover, _account);
                    }

                    //_squareCover = await _uploadedFileAzure.SaveFileAzure(model.SquareCover, _account);
                    _squareCover = _uploadedFile.UploadedFileImage(model.SquareCover);
                }
                else
                {
                    _squareCover = _product.SquareCover;
                }

                _product.Name = model.Name;
                _product.CoverPage = _coverPage;
                _product.SquareCover = _squareCover;
                _product.Description = model.Description;
                _product.Price = model.Price;
                _product.Discounts = model.Discounts;
                _product.Increments = model.Increments;
                _product.PersonNumber = model.PersonNumber;
                _product.AmountSupported = model.AmountSupported;
                _product.Statud = model.Statud;
                _product.AmountSupported = model.AmountSupported;
                _product.PlaceId = model.PlaceId;
                _product.UpdateDate = DateTime.Now;

                await _context.SaveChangesAsync();

                if (model.Gallery != null)
                {

                    var _getGalleries = _galleryService.GetAll().Where(x => x.ProducId == id).ToList();

                    if (_getGalleries.Count > 0)
                    {
                        foreach (var item in _getGalleries)
                        {
                            //await _uploadedFileAzure.DeleteFile(item.NameImage, _account);

                            _uploadedFile.DeleteConfirmed(item.NameImage);

                            _context.Remove(new Gallery
                            {
                                GalleryId = item.GalleryId
                            });
                            await _context.SaveChangesAsync();

                        }
                    }

                    if (model.Gallery.Any())
                    {
                        List<string> _uploadGalleries = _uploadedFile.UploadedMultipleFileImage(model.Gallery);
                        int _accountant = 0;

                        foreach (var item in model.Gallery)
                        {

                            var _gallery = new Gallery
                            {
                                ProducId = _product.ProductId,
                                NameImage = _uploadGalleries[_accountant]
                            };

                            await _context.AddAsync(_gallery);
                            await _context.SaveChangesAsync();
                            _mapper.Map<GalleryDto>(_gallery);

                            _accountant++;
                        }

                    }

                }

                //Método para AZURE

                //if (model.Gallery != null)
                //{

                //    var _getGalleries = _galleryService.GetAll().Where(x => x.ProducId == id).ToList();

                //    if (_getGalleries.Count > 0)
                //    {
                //        foreach (var item in _getGalleries)
                //        {
                //            await _uploadedFileAzure.DeleteFile(item.NameImage, _account);

                //            _context.Remove(new Gallery
                //            {
                //                GalleryId = item.GalleryId
                //            });
                //            await _context.SaveChangesAsync();

                //        }
                //    }

                //    string[] _uploadGalleries = new string[model.Gallery.Count()];
                //    int _accountant = 0;

                //    foreach (var item in model.Gallery)
                //    {
                //        _uploadGalleries[_accountant] = await _uploadedFileAzure.SaveFileAzure(item, _account);

                //        var _gallery = new Gallery
                //        {
                //            ProducId = _product.ProductId,
                //            NameImage = _uploadGalleries[_accountant]
                //        };

                //        await _context.AddAsync(_gallery);
                //        await _context.SaveChangesAsync();
                //        _mapper.Map<GalleryDto>(_gallery);

                //        _accountant++;
                //    }

                //}

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
        }

        public async Task DeleteConfirmed(int _id, string _cover, string _squareCover)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var _getGalleries = _galleryService.GetAll().Where(x => x.ProducId == _id).ToList();

                    if (_getGalleries.Count > 0)
                    {
                        foreach (var item in _getGalleries)
                        {
                            //await _uploadedFileAzure.DeleteFile(item.NameImage, _account);
                            _uploadedFile.DeleteConfirmed(item.NameImage);

                            _context.Remove(new Gallery
                            {
                                GalleryId = item.GalleryId
                            });
                            await _context.SaveChangesAsync();

                        }
                    }

                    if (_cover != null)
                    {
                        //await _uploadedFileAzure.DeleteFile(_cover, _account);
                        _uploadedFile.DeleteConfirmed(_cover);
                    }

                    if (_squareCover != null)
                    {
                        //await _uploadedFileAzure.DeleteFile(_cover, _account);
                        _uploadedFile.DeleteConfirmed(_squareCover);
                    }

                    _context.Remove(new Product
                    {
                        ProductId = _id

                    });
                    await _context.SaveChangesAsync();

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

        //Este método es llamado desde la vista Details de BLOG
        public async Task<IEnumerable<Product>> WhereToSleep()
        {
            var _getAll = _context.Products
                .Include(p => p.Place)
                .ThenInclude(c => c.Category)
                .Where(x => x.Statud == true && x.Place.Category.Name == "souvenir");

            return (await _getAll.ToListAsync());
        }

        public bool DuplicaName(string name)
        {

            var urlName = _context.Products
                .Where(x => x.Name == name);

            return urlName.Any();
        }

    }
}

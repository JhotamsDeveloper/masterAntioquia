using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTOs;
using Persisten.Database;
using Service.Commons;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ITouristExcursionsService
    {
        Task<IEnumerable<TouristExcursionsDto>> GetAll();
        Task<TouristExcursionsDto> Details(int? id);
        Task<TouristExcursionsDto> Details(string urlName);
        Task<ProductDto> Create(TouristExcursionsCreateDto model);
        Task Edit(int id, TouristExcursionsEditDto model);
        Task<TouristExcursionsDto> GetById(int? id);
        Task DeleteConfirmed(int _id, string _cover, string _squareCover);
        Task<IEnumerable<TouristExcursionsDto>> Tours();
        bool ProductExists(int id);
        Boolean DuplicaName(string name);
    }

    public class TouristExcursionsService : ITouristExcursionsService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFormatString _formatString;
        private readonly IGalleryService _galleryService;
        private readonly IUploadedFileAzure _uploadedFileAzure;
        private readonly IUploadedFile _uploadedFile;
        private readonly string _account = "tour";

        public TouristExcursionsService(
            ApplicationDbContext context,
            IMapper mapper,
            IGalleryService galleryService,
            IUploadedFileAzure uploadedFileAzure,
            IUploadedFile uploadedFile,
            IFormatString formatString)
        {
            _context = context;
            _mapper = mapper;
            _galleryService = galleryService;
            _uploadedFileAzure = uploadedFileAzure;
            _uploadedFile = uploadedFile;
            _formatString = formatString;
        }

        #region "BACKEND"
        public async Task<IEnumerable<TouristExcursionsDto>> GetAll()
        {

            var _getAll = _context.Products
                .AsNoTracking()
                .Include(a => a.Place)
                .ThenInclude(b => b.Category)
                .Where(c => c.Place.Category.Name == "tour");

            //Falta agregar un número de referencia
            //Falta tambien agregar la fecha de partida (La tiene en la base de datos pero hay que hacerle una tabla a parte)
            var _model = from t in _getAll
                         select new TouristExcursionsDto
                         {
                             ProductId = t.ProductId,
                             Reference = t.Place.Nit,
                             Name = t.Name,
                             ProductUrl = t.ProductUrl,
                             Description = t.Description,
                             CreationDate = t.CreationDate,
                             UpdateDate = t.UpdateDate,
                             SquareCover = t.SquareCover,
                             Business = t.Place.Name,
                             City = t.Place.City,
                             TourIsUrban = t.TourIsUrban,
                             Statud = t.Statud
                         };

            return await _model.ToListAsync();
        }

        public async Task<TouristExcursionsDto> Details(int? id)
        {

            var _tour = _mapper.Map<TouristExcursionsDto>(
                    await _context.Products
                    .FirstOrDefaultAsync(m => m.ProductId == id)
                );


            return _mapper.Map<TouristExcursionsDto>(_tour);
        }

        public async Task<TouristExcursionsDto> Details(string urlName)
        {
            var _tour = _mapper.Map<TouristExcursionsDto>(
                    await _context.Products
                    .Include(p=>p.Place)
                    .Include(i=>i.Galleries)
                    .Where(s => s.Statud == true)
                    .FirstOrDefaultAsync(m => m.ProductUrl == urlName)

                );

            return _mapper.Map<TouristExcursionsDto>(_tour);
        }

        public async Task<ProductDto> Create(TouristExcursionsCreateDto model)
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
                        Name = model.Name,
                        ProductUrl = _url,
                        CoverPage = _coverPage,
                        SquareCover = _squareCover,
                        Description = model.Description,
                        TourIsUrban = model.TourIsUrban,
                        Statud = model.Statud,
                        PlaceId = model.PlaceId,

                        CreationDate = _fechaActual
                    };

                    await _context.AddAsync(_product);
                    await _context.SaveChangesAsync();

                    _mapper.Map<ProductDto>(_product);

                    //List<string> _uploadGalleries = _uploadedFile.UploadedMultipleFileImage(model.Gallery);

                    if (model.Gallery.Any())
                    {
                        string[] _uploadGalleries = new string[model.Gallery.Count()];
                        int _accountant = 0;

                        foreach (var item in model.Gallery)
                        {
                            //_uploadGalleries[_accountant] = await _uploadedFileAzure.SaveFileAzure(item, _account);
                            _uploadGalleries[_accountant] = _uploadedFile.UploadedFileImage(item);

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


                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }

            return null;

        }

        public async Task Edit(int id, TouristExcursionsEditDto model)
        {
            using (var transaction = _context.Database.BeginTransaction())

            {
                try
                {
                    DateTime _dateUpdate = DateTime.Now;
                    var _tour = await _context.Products.SingleAsync(x => x.ProductId == id);
                    var _coverPage = "";
                    var _squareCover = "";
                    
                    if (model.CoverPage != null)
                    {
                        if (_tour.CoverPage != null)
                        {
                            //await _uploadedFileAzure.DeleteFile(_tour.CoverPage, _account);
                            _uploadedFile.DeleteConfirmed(_tour.CoverPage);
                        }

                        //_coverPage = await _uploadedFileAzure.SaveFileAzure(model.CoverPage, _account);
                        _coverPage = _uploadedFile.UploadedFileImage(model.CoverPage);
                    }
                    else
                    {
                        _coverPage = _tour.CoverPage;
                    }

                    if (model.SquareCover != null)
                    {
                        if (_tour.SquareCover != null)
                        {
                            //await _uploadedFileAzure.DeleteFile(_tour.SquareCover, _account);
                            _uploadedFile.DeleteConfirmed(_tour.SquareCover);
                        }

                        //_squareCover = await _uploadedFileAzure.SaveFileAzure(model.SquareCover, _account);
                        _squareCover = _uploadedFile.UploadedFileImage(model.SquareCover);
                    }
                    else
                    {
                        _squareCover = _tour.SquareCover;
                    }


                    _tour.Name = model.Name;
                    _tour.CoverPage = _coverPage;
                    _tour.SquareCover = _squareCover;
                    _tour.Description = model.Description;
                    _tour.TourIsUrban = model.TourIsUrban;
                    _tour.PlaceId = model.PlaceId;
                    _tour.Statud = model.Statud;
                    _tour.UpdateDate = DateTime.Now;

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

                        string[] _uploadGalleries = new string[model.Gallery.Count()];
                        int _accountant = 0;

                        foreach (var item in model.Gallery)
                        {
                            //_uploadGalleries[_accountant] = await _uploadedFileAzure.SaveFileAzure(item, _account);

                            _uploadGalleries[_accountant] = _uploadedFile.UploadedFileImage(item);

                            var _gallery = new Gallery
                            {
                                ProducId = _tour.ProductId,
                                NameImage = _uploadGalleries[_accountant]
                            };

                            await _context.AddAsync(_gallery);
                            await _context.SaveChangesAsync();
                            _mapper.Map<GalleryDto>(_gallery);

                            _accountant++;
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

        public async Task<TouristExcursionsDto> GetById(int? id)
        {
            return _mapper.Map<TouristExcursionsDto>(
                await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductId == id)
                );
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

        public bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        #endregion

        #region "FRONTEND"
        public async Task<IEnumerable<TouristExcursionsDto>> Tours()
        {

            var _getAll = _context.Products
                .AsNoTracking()
                .Include(a => a.Place)
                .ThenInclude(b => b.Category)
                .Where(c => c.Place.Category.Name == "tour" 
                && c.Statud == true
                && c.Place.State == true);

            //Falta agregar un número de referencia
            //Falta tambien agregar la fecha de partida (La tiene en la base de datos pero hay que hacerle una tabla a parte)

            var _model = from t in _getAll
                         select new TouristExcursionsDto
                         {
                             ProductId = t.ProductId,
                             Reference = t.Place.Nit,
                             Name = t.Name,
                             ProductUrl = t.ProductUrl,
                             TourIsUrban = t.TourIsUrban,
                             Description = t.Description,
                             CreationDate = t.CreationDate,
                             UpdateDate = t.UpdateDate,
                             SquareCover = t.SquareCover,
                             Business = t.Place.Name,
                             City = t.Place.City,
                             Statud = t.Statud
                         };

            return await _model.ToListAsync();
        }

        public bool CategoryExists(int id)
        {

            return _context.Places.Any(e => e.PlaceId == id);
        }
        public Boolean DuplicaName(string name)
        {

            var urlName = _context.Products
                .Where(x => x.Name == name);

            return urlName.Any();
        }

        #endregion
    }
}

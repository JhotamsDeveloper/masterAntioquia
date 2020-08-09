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
        Task<ProductDto> Create(TouristExcursionsCreateDto model);
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
        private readonly string _account = "tour";

        public TouristExcursionsService(
            ApplicationDbContext context,
            IMapper mapper,
            IGalleryService galleryService,
            IUploadedFileAzure uploadedFileAzure,
            IFormatString formatString)
        {
            _context = context;
            _mapper = mapper;
            _galleryService = galleryService;
            _uploadedFileAzure = uploadedFileAzure;
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
                             Reference = t.Place.Nit,
                             Name = t.Name,
                             ProductUrl = t.ProductUrl,
                             Description = t.Description,
                             CreationDate = t.CreationDate,
                             UpdateDate = t.UpdateDate,
                             SquareCover = t.SquareCover,
                             Business = t.Place.Name,
                             City = t.Place.City,
                             State = t.Statud
                         };

            return await _model.ToListAsync();
        }

        public async Task<TouristExcursionsDto> Details(int? id)
        {

            var _tour = _mapper.Map<TouristExcursionsDto>(
                    await _context.Products
                    .FirstOrDefaultAsync(m => m.PlaceId == id)
                );


            return _mapper.Map<TouristExcursionsDto>(_tour);
        }

        public async Task<ProductDto> Create(TouristExcursionsCreateDto model)
        {

            using (var transaction = _context.Database.BeginTransaction())

            {
                try
                {

                    var _coverPage = await _uploadedFileAzure.SaveFileAzure(model.CoverPage, _account);
                    var _squareCover = await _uploadedFileAzure.SaveFileAzure(model.SquareCover, _account);

                    var _fechaActual = DateTime.Now;
                    var _url = _formatString.FormatUrl(model.Name);

                    var _product = new Product
                    {
                        Name = model.Name,
                        ProductUrl = _url,
                        CoverPage = _coverPage,
                        SquareCover = _squareCover,
                        Description = model.Description,
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
                            _uploadGalleries[_accountant] = await _uploadedFileAzure.SaveFileAzure(item, _account);

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


        #endregion
        
        #region "FRONTEND"
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

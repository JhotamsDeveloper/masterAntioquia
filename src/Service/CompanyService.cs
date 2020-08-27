using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTOs;
using Persisten.Database;
using Service.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ICompanyService
    {
        Task<IEnumerable<Place>> GetAll();
        Task<PlaceDto> Details(string nameHotel);
        Task<IEnumerable<Product>> GetProduct(int place);
        Task<IEnumerable<Product>> DetailProducts(int idPlace);
        Task<ReviewDto> CreateReviews(ReviewsCreateDto model);
        Task<IEnumerable<Review>> ReviewsPlaces(int idPlace);
        int TotalReviews(int idPlace);
    }

    public class CompanyService : ICompanyService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUploadedFile _uploadedFile;
        private readonly UserManager<IdentityUser> _userManager;

        public CompanyService(  ApplicationDbContext context,
                                IUploadedFile uploadedFile,
                                UserManager<IdentityUser> userManager,
                                IMapper mapper)
        {
            _context = context;
            _uploadedFile = uploadedFile;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Place>> GetAll()
        {
            var _listAliados = _context.Places
                .AsNoTracking()
                .Include(p=>p.Category)
                .Where(x => x.State == true);
            return (await _listAliados.ToListAsync());
        }

        public async Task<PlaceDto> Details(string urlName)
        {
            var _place = _mapper.Map<PlaceDto>(
                    await _context.Places
                    .AsNoTracking()
                    .Where(s => s.State == true)
                    .FirstOrDefaultAsync(m => m.NameUrl == urlName)
                    );

            return _mapper.Map<PlaceDto>(_place);
        }

        public async Task<IEnumerable<Product>> GetProduct(int place)
        {
            var _listProducts = _context.Products
                                .AsNoTracking()
                                .Where(X => X.Statud == true && X.PlaceId == place);
            return (await _listProducts.ToListAsync());
        }

        public async Task<IEnumerable<Product>> DetailProducts(int idPlace)
        {
            var _listAliados = _context.Products
                .AsNoTracking()
                .Where(x => x.Statud == true && x.PlaceId == idPlace)
                .Take(10);
            return (await _listAliados.ToListAsync());
        }

        public async Task<ReviewDto> CreateReviews(ReviewsCreateDto model)
        {

            var _review = new Review
            {
                TittleReview = model.TittleReview,
                Description = model.DescriptionReview,
                Assessment = model.AssessmentReview,
                UserName = model.UserNameReview.Substring(0,7),
                PlaceId = model.PlaceIdReview,
                ReviewCreateDate = DateTime.Now
            };

            await _context.AddAsync(_review);
            await _context.SaveChangesAsync();

            //Método para guardar en wwwroot
            List<string> _uploadGalleries = _uploadedFile.UploadedMultipleFileImage(model.GalleryReview);

            for (int i = 0; i < _uploadGalleries.Count; i++)
            {
                var _gallery = new Gallery
                {
                    ReviewsId = _review.ReviewID,
                    NameImage = _uploadGalleries[i]
                };

                await _context.AddAsync(_gallery);
                await _context.SaveChangesAsync();
                _mapper.Map<GalleryDto>(_gallery);

            }

            return _mapper.Map<ReviewDto>(_review);

        }

        public async Task<IEnumerable<Review>> ReviewsPlaces(int idPlace)
        {
            var _listAliados = _context.Reviews
                .AsNoTracking()
                .Include(a=>a.Galleries)
                .Where(x => x.PlaceId == idPlace)
                .OrderByDescending(x=>x.ReviewCreateDate)
                .Take(3);
            return (await _listAliados.ToListAsync());
        }

        public int TotalReviews(int idPlace)
        {
            var _totalReviews = _context.Reviews
                                .Count(x => x.PlaceId == idPlace);
            return _totalReviews;
        }

    }
}

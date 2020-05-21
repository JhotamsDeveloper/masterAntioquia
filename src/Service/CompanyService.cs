using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTOs;
using Persisten.Database;
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
    }

    public class CompanyService : ICompanyService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CompanyService(ApplicationDbContext context,
                              IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Place>> GetAll()
        {
            var _listAliados = _context.Places.Where(X => X.State == true);
            return (await _listAliados.ToListAsync());
        }

        public async Task<PlaceDto> Details(string urlName)
        {
            var _place = _mapper.Map<PlaceDto>(
                    await _context.Places
                    .Include(c => c.Products)
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
    }
}

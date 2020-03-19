using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTOs;
using Persisten.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<ProductDto> Details(int? id);
    }

    public class ProductService : IProductService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            var applicationDbContext = _context.Products.Include(p => p.Place);
            return (await applicationDbContext.ToListAsync());
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
    }
}

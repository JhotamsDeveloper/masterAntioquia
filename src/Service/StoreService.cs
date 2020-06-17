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

namespace Service
{
    public interface IStoreService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<IEnumerable<Product>> StoreProducts();
    }

    public class StoreService : IStoreService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StoreService(ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        #endregion

    }
}

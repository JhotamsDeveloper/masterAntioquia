using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model.DTOs;
using Persisten.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IGenericServicio 
    {
        Task<IEnumerable<ProductDto>> NewsList(int quantity);
    }
    public class GenericServicio : IGenericServicio
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GenericServicio(ApplicationDbContext context,
            IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> NewsList(int quantity)
        {
            var _newList = _context.Products
                .AsNoTracking()
                .Include(a => a.Place)
                .ThenInclude(b => b.Category)
                .Where(c => c.Statud == true
                            && c.Place.Category.Name == "Hotel"
                            && c.Place.State == true)
                .OrderBy(d => Guid.NewGuid())
                .Take(quantity);
            
            NumberFormatInfo nfi = new CultureInfo("es-CO", false).NumberFormat;
            nfi = (NumberFormatInfo)nfi.Clone();
            nfi.CurrencySymbol = "$";

            var _modelo = from b in _newList
                          select new ProductDto
                          {
                              ProductId = b.ProductId,
                              Name = b.Name,
                              ProductUrl = b.ProductUrl,
                              Description = b.Description,
                              Price = string.Format(nfi, "{0:C0}", b.Price),
                              City = b.Place.Name,
                              CoverPage = b.CoverPage,
                              SquareCover = b.SquareCover,
                              UpdateDate = b.UpdateDate,
                              Statud = b.Statud
                          };

            return (await _modelo.ToListAsync());
        }
    }
}

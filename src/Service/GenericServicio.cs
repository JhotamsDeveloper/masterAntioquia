using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public interface IGenericServicio 
    {
        Task<IEnumerable<ProductDto>> NewsList(int quantity);
        Task Execute(string subject, string message, string email);
    }
    public class GenericServicio : IGenericServicio
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSendGrid _emailSendGrid;
        private readonly IMapper _mapper;

        public GenericServicio(
            ApplicationDbContext context,
            IEmailSendGrid emailSendGrid,
            IMapper mapper) 
        {
            _context = context;
            _emailSendGrid = emailSendGrid;
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
            //Price = string.Format(nfi, "{0:C0}", b.Price),

            var _modelo = from b in _newList
                          select new ProductDto
                          {
                              ProductId = b.ProductId,
                              Name = b.Name,
                              ProductUrl = b.ProductUrl,
                              Description = b.Description,
                              Price = b.Price,
                              City = b.Place.Name,
                              CoverPage = b.CoverPage,
                              SquareCover = b.SquareCover,
                              UpdateDate = b.UpdateDate,
                              Statud = b.Statud
                          };

            return (await _modelo.ToListAsync());
        }

        public Task Execute(string subject, string message, string email)
        {
            return _emailSendGrid.Execute(subject, message, email);
        }
    }
}

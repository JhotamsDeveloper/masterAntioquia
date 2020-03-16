using AutoMapper;
using Persisten.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public interface IProductService
    {

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
    }
}

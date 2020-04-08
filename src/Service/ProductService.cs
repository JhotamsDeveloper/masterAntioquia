﻿using AutoMapper;
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

namespace Service
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<ProductDto> Details(int? id);
        Task<ProductDto> Create(ProductCreateDto model);
        Task<ProductDto> GetById(int? id);
        Task<ICollection<Gallery>> ListGalleries();

        IEnumerable<Product> GetAllTest();
    }

    public class ProductService : IProductService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IUploadedFile _uploadedFile;
        private readonly IMapper _mapper;

        public ProductService(ApplicationDbContext context,
            IUploadedFile uploadedFile,
            IMapper mapper)
        {
            _context = context;
            _uploadedFile = uploadedFile;
            _mapper = mapper;
        }


        public async Task<IEnumerable<Product>> GetAll()
        {
            var _getAll = _context.Products.Include(p => p.Place);
            return (await _getAll.ToListAsync());
        }

        public IEnumerable<Product> GetAllTest()
        {
            return _context.Products.ToList();
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

                    var _coverPage = _uploadedFile.UploadedFileImage(model.CoverPage);
                    var _fechaActual = DateTime.Now;

                    var _product = new Product
                    {
                        Name = model.Name,
                        CoverPage = _coverPage,
                        Description = model.Description,
                        Price = model.Price.ToString().Trim(),
                        HighPrice = model.HighPrice,
                        HalfPrice = model.HalfPrice,
                        LowPrice = model.LowPrice,
                        Discounts = model.Discounts,
                        Statud = model.Statud,
                        PlaceId = model.PlaceId,
                        
                        CreationDate = _fechaActual
                    };

                    await _context.AddAsync(_product);
                    await _context.SaveChangesAsync();

                    _mapper.Map<ProductDto>(_product);

                    List<string> _uploadGalleries = _uploadedFile.UploadedMultipleFileImage(model.Gallery);

                    for (int i = 0; i < _uploadGalleries.Count; i++)
                    {
                        var _gallery = new Gallery
                        {
                            ProducId = _product.ProductId,
                            NameImage = _uploadGalleries[i]
                        };

                        await _context.AddAsync(_gallery);
                        await _context.SaveChangesAsync();
                        _mapper.Map<GalleryDto>(_gallery);

                    }

                    transaction.Commit();
                }
                catch (Exception)
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
                .Include(x=>x.Galleries)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductId == id)
                );

        }

        public async Task<ICollection<Gallery>> ListGalleries()
        {
            var _list = _context.Galleries;

            return (await _list.ToListAsync());
        }

        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Galleries.ToListAsync());
        //}

    }
}

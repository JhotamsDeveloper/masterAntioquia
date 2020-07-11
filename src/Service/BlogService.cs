﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTOs;
using Model.DTOs.CustomValidations;
using Persisten.Database;
using Service.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IBlogService 
    {
        Task<BlogDto> Details(int? id);
        Task<BlogDto> Details(string name);
        Task<BlogDto> Create(BlogCreateDto model);
        Task<BlogDto> GetById(int? id);
        Task Edit(int id, BlogEditDto model);
        Task DeleteConfirmed(int _id, string _squareCover, string _cover);
        Task<BlogDto> BlogUrl(string _blogUrl);
        bool BlogExists(int? id);
        Task<IEnumerable<BlogDto>> Blog();
    }
    public class BlogService : IBlogService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IGalleryService _galleryService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IUploadedFile _uploadedFile;
        private readonly IFormatString _formatString;

        public BlogService(
            ApplicationDbContext context,
            IMapper mapper,
            IGalleryService galleryService,
            IProductService productService,
            ICategoryService categoryService,
            IUploadedFile uploadedFile,
            IFormatString formatString)
        {
            _context = context;
            _mapper = mapper;
            _galleryService = galleryService;
            _productService = productService;
            _categoryService = categoryService;
            _uploadedFile = uploadedFile;
            _formatString = formatString;
        }

        #region "BackEnd"

        public async Task<BlogDto> Details(int? id)
        {

            var _blog = _mapper.Map<BlogDto>(
                    await _context.Events
                    .Include(p => p.Place)
                    .FirstOrDefaultAsync(m => m.EventId == id)
                );

            return _mapper.Map<BlogDto>(_blog);
        }

        public async Task<BlogDto> Details(string name)
        {
            

            var _producGuid = await _context.Products
                .AsNoTracking()
                .Include(a => a.Place)
                .ThenInclude(b => b.Category)
                .Where(c =>    c.Statud == true
                            && c.Place.Category.Name == "Hotel"
                            && c.Place.State == true)
                .OrderBy(d => Guid.NewGuid())
                .Take(5)
                .ToListAsync();

            var _blog = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Name == name && m.State == true);

            var _modelo = new BlogDto
                {
                    EventId = _blog.EventId,
                    Name = _blog.Name,
                    BlogUrl = _blog.EventUrl,
                    Description = _blog.Description,
                    Author = _blog.Author,
                    CoverPage = _blog.CoverPage,
                    SquareCover = _blog.SquareCover,
                    UpdateDate = _blog.UpdateDate,
                    State = _blog.State,
                    Products = _producGuid
            };


            return _mapper.Map<BlogDto>(_modelo);
        }
        public async Task<BlogDto> Create(BlogCreateDto model)
        {

            var _coverPage = _uploadedFile.UploadedFileImage(model.CoverPage);
            var _squareCover = _uploadedFile.UploadedFileImage(model.SquareCover);
            var _fechaActual = DateTime.Now;
            var _url = _formatString.FormatUrl(model.Name);

            var _blog = new Event
            {
                Name = model.Name,
                EventUrl = _url,
                CoverPage = _coverPage,
                SquareCover = _squareCover,
                Description = model.Description,
                State = model.State,
                Author = model.Author,
                CategoryId = 4,
                CreationDate = _fechaActual,
                UpdateDate = _fechaActual
            };

            await _context.AddAsync(_blog);
            await _context.SaveChangesAsync();

            return _mapper.Map<BlogDto>(_blog);

        }

        public async Task<BlogDto> GetById(int? id)
        {

            return _mapper.Map<BlogDto>(
                await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EventId == id)
                );

        }

        public async Task Edit(int id, BlogEditDto model)
        {
            DateTime _dateUpdate = DateTime.Now;

            var _blog = await _context.Events.SingleAsync(x => x.EventId == id);

            string _coverPage;
            string _squareCover;

            if (model.CoverPage != null)
            {
                _coverPage = _uploadedFile.UploadedFileImage(_blog.CoverPage, model.CoverPage);
            }
            else
            {
                _coverPage = _blog.CoverPage;
            }

            if (model.SquareCover != null)
            {
                _squareCover = _uploadedFile.UploadedFileImage(_blog.SquareCover, model.SquareCover);
            }
            else
            {
                _squareCover = _blog.SquareCover;
            }

            _blog.Name = model.Name;
            _blog.CoverPage = _coverPage;
            _blog.SquareCover = _squareCover;
            _blog.Description = model.Description;
            _blog.State = model.State;
            _blog.Author = model.Author;
            _blog.UpdateDate = _dateUpdate;

            await _context.SaveChangesAsync();

        }

        public async Task DeleteConfirmed(int _id, string _squareCover, string _cover)
        {
            if (_cover != null)
            {
                _uploadedFile.DeleteConfirmed(_cover);
            }

            if (_squareCover != null)
            {
                _uploadedFile.DeleteConfirmed(_squareCover);
            }

            _context.Remove(new Event
            {
                EventId = _id

            });
            await _context.SaveChangesAsync();

        }

        public async Task<BlogDto> BlogUrl(string _blogUrl)
        {
            var _blog = _mapper.Map<BlogDto>(
                    await _context.Events
                    .Include(p => p.Place)
                    .Where(s => s.State == true)
                    .FirstOrDefaultAsync(m => m.EventUrl == _blogUrl)

                );

            return _mapper.Map<BlogDto>(_blog);
        }

        public bool BlogExists(int? id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
        
        #endregion

        #region "FrontEnd"
        public async Task<IEnumerable<BlogDto>> Blog()
        {

            var _getAll = _context
                .Events.Include(c=>c.Category)
                .Where(x=>x.Category.Name == "Blog" && x.State == true);

            var _modelo = from b in _getAll
                          select new BlogDto
                          {
                              EventId = b.EventId,
                              Name = b.Name,
                              BlogUrl = b.EventUrl,
                              Description = b.Description,
                              Author = b.Author,
                              CoverPage = b.CoverPage,
                              SquareCover = b.SquareCover,
                              UpdateDate = b.UpdateDate,
                              State = b.State
                          };

            return (await _modelo.ToListAsync());
        }
        #endregion

    }
}

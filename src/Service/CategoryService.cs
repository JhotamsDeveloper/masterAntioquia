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
    public interface ICategoryService
    {
        string CreateCategory(Category category);
        Task<CategoryDto> Create(CategoryCreateDto model);
        List<Category> GetAll();
    }
    public class CategoryService:ICategoryService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(ApplicationDbContext context,
                               IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string CreateCategory(Category category)
        {
            _context.Categorys.Add(category);
            _context.SaveChanges();
            return "Categoria Guardada";
        }
        public async Task<CategoryDto> Create(CategoryCreateDto model)
        {
            var category = new Category
            {
                Name = model.Name,
                Icono = model.Icono,
                Stated = model.Stated
            };

            await _context.AddAsync(category);
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(category);
        }



        public List<Category> GetAll() {
            var catList = _context.Categorys.ToList();
            return catList;
        }
    }
}

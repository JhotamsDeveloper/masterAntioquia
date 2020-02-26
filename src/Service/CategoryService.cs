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
        Task<IEnumerable<Category>> GetAll();
        Task<CategoryDto> Details(int? id);
        Task<CategoryDto> Create(CategoryCreateDto model);
        Task<CategoryDto> Edit(int? id);
        Task Edit(int id, CategoryEditDto model);
        Task<Category> GetById(int? id);
        Task DeleteConfirmed(int id);
        bool CategoryExists(int id);
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

        public async Task<IEnumerable<Category>> GetAll()
        {
            return (await _context.Categorys.ToListAsync());
        }

        public async Task<CategoryDto> Details(int? id)
        {
            var category = _mapper.Map<CategoryDto>(
                    await _context.Categorys
                    .FirstOrDefaultAsync(m => m.CategoryId == id)
                );

            //return (category);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> Create(CategoryCreateDto model)
        {
            var cate = new Category
            {
                Name = model.Name,
                Icono = model.Icono,
                Stated = model.Stated
            };

            await _context.AddAsync(cate);
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(cate);
        }

        public async Task<CategoryDto> Edit(int? id)
        {
            return _mapper.Map<CategoryDto>(
                await _context.Categorys.FindAsync(id));
        }

        public async Task Edit(int id, CategoryEditDto model)
        {
            var cate = await _context.Categorys.SingleAsync(x => x.CategoryId == id);
            
            cate.Name = model.Name;
            cate.Icono = model.Icono;
            cate.Stated = model.Stated;

            await _context.SaveChangesAsync();
        }

        
        public async Task<Category> GetById(int? id)
        {
            return await _context.Categorys.FirstOrDefaultAsync(x => x.CategoryId == id);

                //return _mapper.Map<CategoryDto>(
                    //await _context.Categorys.FindAsync(id)
                //);
        }

        public async Task DeleteConfirmed(int id)
        {
            _context.Remove(new Category
            {
                CategoryId = id
            });

            await _context.SaveChangesAsync();

        }

        public bool CategoryExists(int id)
        {
            return _context.Categorys.Any(e => e.CategoryId == id);
        }
    }
}

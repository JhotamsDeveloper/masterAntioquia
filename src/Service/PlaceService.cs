using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTOs;
using Persisten.Database;
using Service.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    public interface IPlaceService
    {
        Task<IEnumerable<Place>> GetAll();
        Task<PlaceDto> Edit(int? id);
        Task DeleteConfirmed(int id);
        Task<PlaceDto> GetById(int? id);
        Task<PlaceDto> Details(int? id);
        Task<PlaceDto> Create(PlaceCreateDto model);
        Task Edit(int id, PlaceEditDto model);
        Task<PlaceDto> GetByIdDelete(int? id);
        bool CategoryExists(int id);

    }
    public class PlaceService : IPlaceService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;


        public PlaceService(ApplicationDbContext context,
            IHostingEnvironment hostingEnvironment,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IEnumerable<Place>> GetAll()
        {

            var _listPlaces = _context.Places.Include(x => x.Category);
            return (await _listPlaces.ToListAsync());
        }

        public async Task<PlaceDto> Details(int? id)
        {

            var _place = _mapper.Map<PlaceDto>(
                    await _context.Places
                    .Include(c=>c.Category)
                    .FirstOrDefaultAsync(m => m.PlaceId == id)
                );

            return _mapper.Map<PlaceDto>(_place);
        }

        public async Task<PlaceDto> Create(PlaceCreateDto model)
        {
            var _uploadedFile = new UploadedFile(_hostingEnvironment);

            var _coverPage = _uploadedFile.UploadedFileCoverPage(model);
            var _logo = _uploadedFile.UploadedFileLogo(model);
            var _fechaActual = DateTime.Now;

            var _place = new Place
            {
                Nit = model.Nit,
                Name = model.Name,
                Phone = model.Phone,
                Admin = model.Admin,
                Address = model.Address,
                Description = model.Description,
                CoverPage = _coverPage,
                Logo = _logo,
                Contract = model.Contract,
                State = model.State,
                CreationDate = _fechaActual.ToString(),
                CategoryId = model.CategoryId
            };

            await _context.AddAsync(_place);
            await _context.SaveChangesAsync();

            return _mapper.Map<PlaceDto>(_place);
        }

        public string UploadedFile(PlaceCreateDto model)
        {
            string uniqueFileName = null;

            if (model.CoverPage != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.CoverPage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.CoverPage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public async Task<PlaceDto> Edit(int? id)
        {
            return _mapper.Map<PlaceDto>(
                await _context.Places.FindAsync(id));
        }

        public async Task Edit(int id, PlaceEditDto model)
        {
            var _fechaUpdate = DateTime.Now;
            var _place = await _context.Places.SingleAsync(x => x.PlaceId == id);

            _place.Nit = model.Nit;
            _place.Name = model.Name;
            _place.Phone = model.Phone;
            _place.Admin = model.Admin;
            _place.Address = model.Address;
            _place.Description = model.Description;
            _place.CoverPage = model.CoverPage;
            _place.Logo = model.Logo;
            _place.Contract = model.Contract;
            _place.State = model.State;
            _place.UpdateDate = _fechaUpdate.ToString();
            _place.CategoryId = model.CategoryId;


            await _context.SaveChangesAsync();
        }
        public async Task<PlaceDto> GetByIdDelete(int? id)
        {
            return _mapper.Map<PlaceDto>(
                await _context.Places
                .Include(c=>c.CategoryId)
                .FirstOrDefaultAsync(p=>p.PlaceId == id));
        }
        public async Task<PlaceDto> GetById(int? id)
        {
            return _mapper.Map<PlaceDto>(
                await _context.Places.FindAsync(id));
        }
        public async Task DeleteConfirmed(int id)
        {
            _context.Remove(new Place
            {
                PlaceId = id
            });

            await _context.SaveChangesAsync();
        }
        public bool CategoryExists(int id)
        {
            return _context.Places.Any(e => e.PlaceId == id);
        }

    }
}

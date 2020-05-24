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
        Task<PlaceDto> Create(PlaceCreateDto model);
        Task<PlaceDto> GetById(int? id);
        Task<PlaceDto> Details(int? id);
        Task<PlaceDto> Edit(int? id);
        Task Edit(int id, PlaceEditDto model);

        Task DeleteConfirmed(int id, string _cover, string _logo);
        bool CategoryExists(int id);
        Task<IEnumerable<Place>> GetAliados();
        Task<PlaceDto> DetalleHotel(string nameHotel);

        Boolean DuplicaName(string name);

    }
    public class PlaceService : IPlaceService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUploadedFile _uploadedFile;

        public PlaceService(ApplicationDbContext context,
            IUploadedFile uploadedFile,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _uploadedFile = uploadedFile;

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

            var _coverPage = _uploadedFile.UploadedFileImage(model.CoverPage);
            var _logo = _uploadedFile.UploadedFileImage(model.Logo);

            var _fechaActual = DateTime.Now;
            var _url = FormatString(model.Name);
            var _place = new Place
            {
                Nit = model.Nit,
                Name = model.Name,
                NameUrl = _url.ToLower(),
                Phone = model.Phone,
                Admin = model.Admin,
                Email = model.Email,
                City = model.City,
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


        public async Task<PlaceDto> Edit(int? id)
        {
            return _mapper.Map<PlaceDto>(
                await _context.Places.FindAsync(id)
                );
        }

        public async Task Edit(int id, PlaceEditDto model)
        {
            var _fechaUpdate = DateTime.Now;

            var _place = await _context.Places.SingleAsync(x => x.PlaceId == id);

            var _coverPage = _uploadedFile.UploadedFileImage(_place.CoverPage, model.CoverPage);
            var _logo = _uploadedFile.UploadedFileImage(_place.Logo, model.Logo);

            _place.Nit = model.Nit;
            _place.Phone = model.Phone;
            _place.Admin = model.Admin;
            _place.Address = model.Address;
            _place.Email = model.Email;
            _place.City = model.City;
            _place.Description = model.Description;
            _place.CoverPage = _coverPage;
            _place.Logo = _logo;
            _place.Contract = model.Contract;
            _place.State = model.State;
            _place.UpdateDate = _fechaUpdate.ToString();
            _place.CategoryId = model.CategoryId;

            await _context.SaveChangesAsync();

        }
        public async Task<PlaceDto> GetById(int? id)
        {

            //return _mapper.Map<PlaceDto> (
            //    await _context.Places
            //    .FindAsync(id));

            return _mapper.Map<PlaceDto>(
                await _context.Places
                .AsNoTracking()
                .FirstOrDefaultAsync(x=>x.PlaceId == id)
                );

            //var place = await _context.Places
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(x => x.PlaceId == id);
        }

        public async Task DeleteConfirmed(int _id, string _cover, string _logo)
        {

            if (_logo != null || _cover != null)
            {
                _uploadedFile.DeleteConfirmed(_cover);
                _uploadedFile.DeleteConfirmed(_logo);
            }

            _context.Remove(new Place
            {
                PlaceId = _id
            });

            await _context.SaveChangesAsync();

        }
        public bool CategoryExists(int id)
        {
            return _context.Places.Any(e => e.PlaceId == id);
        }

        public async Task<IEnumerable<Place>> GetAliados()
        {

            var _listAliados = _context.Places.Where(X=>X.State == true);
            return (await _listAliados.ToListAsync());
        }

        public async Task<PlaceDto> DetalleHotel(string nameHotel)
        {
            var _place = _mapper.Map<PlaceDto>(
                    await _context.Places
                    .Include(c => c.Category)
                    .Where(s=>s.State == true)
                    .FirstOrDefaultAsync(m => m.Name == nameHotel)
                    
                );

            return _mapper.Map<PlaceDto>(_place);
        }


        private String FormatString(String texto)
        {
            var original = "ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýÿ ";
            // Cadena de caracteres ASCII que reemplazarán los originales.
            var ascii = "AAAAAAACEEEEIIIIDNOOOOOOUUUUYBaaaaaaaceeeeiiiionoooooouuuuyy-";
            var output = texto;
            for (int i = 0; i < original.Length; i++)
            {
                // Reemplazamos los caracteres especiales.

                output = output.Replace(original[i], ascii[i]);

            }

            return output;
        }

        public Boolean DuplicaName(string name)
        {

            var urlName = _context.Places
                .Where(x => x.Name == name);

            return urlName.Any();
        }
    }
}

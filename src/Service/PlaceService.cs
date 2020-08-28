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
using Microsoft.AspNetCore.Identity;

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
        Task DeleteConfirmed(int id, string _cover, string _logo, string _squareCover);
        bool CategoryExists(int id);
        Task<IEnumerable<Place>> GetAliados();
        Task<IEnumerable<Place>> Restaurant();

        Boolean DuplicaName(string name);
        Task<IEnumerable<LogosDto>> Logos();
    }
    public class PlaceService : IPlaceService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUploadedFile _uploadedFile;

        //Variables de Azure
        //private readonly IUploadedFileAzure _uploadedFileAzure;
        //private readonly string _account = "places";

        public PlaceService(ApplicationDbContext context,
            IUploadedFile uploadedFile,
            IUploadedFileAzure uploadedFileAzure,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

            _uploadedFile = uploadedFile;
            //_uploadedFileAzure = uploadedFileAzure;

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

            //var _coverPage = await _uploadedFileAzure.SaveFileAzure(model.CoverPage, _account);
            //var _squareCover = await _uploadedFileAzure.SaveFileAzure(model.SquareCover, _account);
            //var _logo = await _uploadedFileAzure.SaveFileAzure(model.Logo, _account);

            var _coverPage = _uploadedFile.UploadedFileImage(model.CoverPage);
            var _squareCover = _uploadedFile.UploadedFileImage(model.SquareCover);
            var _logo = _uploadedFile.UploadedFileImage(model.Logo);

            var _fechaActual = DateTime.Now;
            var _url = FormatString(model.Name);
            var _place = new Place
            {
                Nit = model.Nit.Trim(),
                Name = model.Name.Trim(),
                NameUrl = _url.ToLower(),
                Phone = model.Phone.Trim(),
                Admin = model.Admin.Trim(),
                Email = model.Email.Trim(),
                City = model.City.Trim(),
                urban = model.urban,
                Address = model.Address.Trim(),
                Description = model.Description.Trim(),
                CoverPage = _coverPage,
                SquareCover = _squareCover,
                Logo = _logo,
                Contract = model.Contract.Trim(),
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
            var _coverPage = "";
            var _squareCover = "";
            var _logo = "";

            var _place = await _context.Places.SingleAsync(x => x.PlaceId == id);

            if (model.CoverPage != null)
            {
                _coverPage = _uploadedFile.UploadedFileImage(model.CoverPage);
            }
            else
            {
                _coverPage = _place.CoverPage;
            }

            if (model.SquareCover != null)
            {
                _squareCover = _uploadedFile.UploadedFileImage(_place.SquareCover, model.SquareCover);
            }
            else
            {
                _squareCover = _place.SquareCover;
            }

            if (model.Logo != null)
            {

                _logo = _uploadedFile.UploadedFileImage(_place.Logo, model.Logo);
            }
            else 
            { 
                _logo = _place.Logo;
            }

            _place.Nit = model.Nit.Trim();
            _place.Phone = model.Phone.Trim();
            _place.Admin = model.Admin.Trim();
            _place.Address = model.Address.Trim();
            _place.Email = model.Email.Trim();
            _place.City = model.City.Trim();
            _place.urban = model.urban;
            _place.Description = model.Description.Trim();
            _place.CoverPage = _coverPage;
            _place.SquareCover = _squareCover;
            _place.Logo = _logo;
            _place.Contract = model.Contract.Trim();
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

        public async Task DeleteConfirmed(int _id, string _cover, string _logo, string _squareCover)
        {

            if (_logo != null)
            {
                //await _uploadedFileAzure.DeleteFile(_logo, _account);
                _uploadedFile.DeleteConfirmed(_logo);

            }
            
            if (_cover != null)
            {
                //await _uploadedFileAzure.DeleteFile(_cover, _account);
                _uploadedFile.DeleteConfirmed(_cover);
            }
            
            if (_squareCover != null)
            {
                //await _uploadedFileAzure.DeleteFile(_squareCover, _account);
                _uploadedFile.DeleteConfirmed(_squareCover);
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

        public async Task<IEnumerable<Place>> Restaurant()
        {

            var _GetAllRestaurante = _context.Places
                .Include(x => x.Category)
                .Where(r=>r.State == true && r.Category.Name == "Restaurante");
            return (await _GetAllRestaurante.ToListAsync());
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

        public async Task<IEnumerable<LogosDto>> Logos()
        {

            var _logos = from p in _context.Places
                         where p.State == true
                         select new LogosDto
                         {
                             LogoName = p.Logo,
                             UrlName = p.NameUrl
                         };

            return (await _logos.ToListAsync());
        }

    }
}

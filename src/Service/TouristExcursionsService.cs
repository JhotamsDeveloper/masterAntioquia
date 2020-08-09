using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model.DTOs;
using Persisten.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ITouristExcursionsService
    {
        Task<IEnumerable<TouristExcursionsDto>> GetAll();
    }

    public class TouristExcursionsService : ITouristExcursionsService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TouristExcursionsService(ApplicationDbContext context,
                              IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TouristExcursionsDto>> GetAll()
        {

            var _getAll = _context.Events
                .AsNoTracking()
                .Include(a => a.Place)
                .ThenInclude(b => b.Category)
                .Where(c => c.Place.Category.Name == "tour" && c.Place.Category.Stated==true);

            //Falta agregar un número de referencia
            //Falta tambien agregar la fecha de partida (La tiene en la base de datos pero hay que hacerle una tabla a parte)
            var _model = from t in _getAll
                         select new TouristExcursionsDto
                         {
                             Reference = t.Place.Nit,
                             Name = t.Name,
                             TourUrl = t.EventUrl,
                             Description = t.Description,
                             EventsDate = t.EventsDate,
                             CreationDate = t.CreationDate,
                             UpdateDate = t.UpdateDate,
                             SquareCover = t.SquareCover,
                             Business = t.Place.Name,
                             City = t.Place.City,
                             State = t.State
                         };

            return await _model.ToListAsync();
        }

        public bool CategoryExists(int id)
        {

            return _context.Places.Any(e => e.PlaceId == id);
        }


    }
}

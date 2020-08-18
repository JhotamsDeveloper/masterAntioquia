using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTOs;
using Persisten.Database;
using Service.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAll();
        Task<EventDto> Details(int? id);
        Task<EventDto> Create(EventCreateDto model);
        Task<EventDto> GetById(int? id);
        Task Edit(int id, EventEditDto model);
        Task DeleteConfirmed(int _id, string _squareCover, string _cover);
        bool EventExists(int? id);
    }

    public class EventService : IEventService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUploadedFile _uploadedFile;
        private readonly IUploadedFileAzure _uploadedFileAzure;
        private readonly IFormatString _formatString;
        private readonly string _account = "events";

        //Constructor
        public EventService(
            ApplicationDbContext context,
            IUploadedFile uploadedFile,
            IUploadedFileAzure uploadedFileAzure,
            IMapper mapper,
            IFormatString formatString)
        {
            _context = context;
            _mapper = mapper;
            _uploadedFile = uploadedFile;
            _uploadedFileAzure = uploadedFileAzure;
            _formatString = formatString;
        }

        #region "BACKEND"
        public async Task<IEnumerable<Event>> GetAll()
        {
            var _getAll = _context.Events
            .Include(c => c.Category)
            .Where(x => x.Category.Name == "Event");
            return (await _getAll.ToListAsync());
        }
        public async Task<EventDto> Details(int? id)
        {

            var _event = _mapper.Map<EventDto>(
                    await _context.Events
                    .Include(p => p.Place)
                    .FirstOrDefaultAsync(m => m.EventId == id)
                );

            return _mapper.Map<EventDto>(_event);
        }

        public async Task<EventDto> Create(EventCreateDto model)
        {

            //var _coverPage = _uploadedFile.UploadedFileImage(model.CoverPage);
            //var _squareCover = _uploadedFile.UploadedFileImage(model.SquareCover);

            var _coverPage = await _uploadedFileAzure.SaveFileAzure(model.CoverPage, _account);
            var _squareCover = await _uploadedFileAzure.SaveFileAzure(model.SquareCover, _account);

            var _fechaActual = DateTime.Now;
            var _url = _formatString.FormatUrl(model.Name);

            var _event = new Event
            {
                Name = model.Name,
                EventUrl = _url,
                CoverPage = _coverPage,
                SquareCover = _squareCover,
                Description = model.Description,
                State = model.State,
                CategoryId = 6,
                CreationDate = _fechaActual,
                UpdateDate = _fechaActual
            };

            await _context.AddAsync(_event);
            await _context.SaveChangesAsync();

            return _mapper.Map<EventDto>(_event);

        }

        public async Task<EventDto> GetById(int? id)
        {

            return _mapper.Map<EventDto>(
                await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EventId == id)
                );

        }

        public async Task Edit(int id, EventEditDto model)
        {
            DateTime _dateUpdate = DateTime.Now;

            var _event = await _context.Events.SingleAsync(x => x.EventId == id);

            string _coverPage;
            string _squareCover;

            if (model.CoverPage != null)
            {
                //_coverPage = _uploadedFile.UploadedFileImage(_blog.CoverPage, model.CoverPage);
                _coverPage = await _uploadedFileAzure.EditFileAzure(model.CoverPage, _event.CoverPage, _account);
            }
            else
            {
                _coverPage = _event.CoverPage;
            }

            if (model.SquareCover != null)
            {
                //_squareCover = _uploadedFile.UploadedFileImage(_blog.SquareCover, model.SquareCover);
                _squareCover = await _uploadedFileAzure.EditFileAzure(model.SquareCover, _event.SquareCover, _account);
            }
            else
            {
                _squareCover = _event.SquareCover;
            }

            _event.Name = model.Name;
            _event.CoverPage = _coverPage;
            _event.SquareCover = _squareCover;
            _event.Description = model.Description;
            _event.State = model.State;
            _event.UpdateDate = _dateUpdate;

            await _context.SaveChangesAsync();

        }


        public async Task DeleteConfirmed(int _id, string _squareCover, string _cover)
        {
            if (_cover != null)
            {
                //_uploadedFile.DeleteConfirmed(_cover);
                await _uploadedFileAzure.DeleteFile(_cover, _account);
            }

            if (_squareCover != null)
            {
                //_uploadedFile.DeleteConfirmed(_squareCover);
                await _uploadedFileAzure.DeleteFile(_squareCover, _account);
            }

            _context.Remove(new Event
            {
                EventId = _id

            });
            await _context.SaveChangesAsync();

        }


        public bool EventExists(int? id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
        #endregion
    }
}

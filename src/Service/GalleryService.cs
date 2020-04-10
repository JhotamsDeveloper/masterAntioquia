using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTOs;
using Persisten.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IGalleryService
    {
        Task<GalleryDto> GetById(int? id);
        IEnumerable<Gallery> GetAll();
    }
    public class GalleryService : IGalleryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GalleryService(
            ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GalleryDto> GetById(int? id)
        {

            return _mapper.Map<GalleryDto>(
                await _context.Galleries
                .SingleAsync(x => x.ProducId == id)
                );

        }

        public IEnumerable<Gallery> GetAll()
        {
            return _context.Galleries;
        }

    }
}

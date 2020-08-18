using AutoMapper;
using GestionAntioquia.Models;
using Model;
using Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAntioquia.Config
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Place, PlaceDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<Product, StoreDto>();
            CreateMap<Product, TouristExcursionsDto>();
            CreateMap<Gallery, GalleryDto>();
            CreateMap<Event, BlogDto>();
            CreateMap<Event, EventDto>();
        }
    }
}

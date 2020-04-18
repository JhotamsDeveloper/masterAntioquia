using AutoMapper;
using GestionAntioquia.Models;
using Microsoft.AspNetCore.Identity;
using Model;
using Model.DTOs;
using Model.Identity;
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
            CreateMap<ApplicationUser, UserDto>();

            CreateMap<Category, CategoryDto>();
            CreateMap<Place, PlaceDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<Gallery, GalleryDto>();
        }
    }
}

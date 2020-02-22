using AutoMapper;
using Model;
using Model.DTOs;
using Service.Commons;

namespace GestionAntioquia.Config
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<DataCollection<Category>, DataCollection<CategoryDto>>();
        }
    }
}

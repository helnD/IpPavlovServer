using AutoMapper;
using Domain;
using UseCases.Categories.SaveCategories;

namespace UseCases.Categories
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryDto, Category>()
                .ForMember(dest => dest.Icon, opt => opt.Ignore());;
        }
    }
}
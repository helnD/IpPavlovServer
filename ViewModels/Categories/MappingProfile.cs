using AutoMapper;
using Domain;
using UseCases.Categories.SaveCategories;
using UseCases.Certificates.SaveCertificates;
using ViewModels.Categories.Models;
using ViewModels.Certificates.Models;

namespace ViewModels.Categories;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryModel>();

        CreateMap<CategoriesModel, SaveCategoriesCommand>();
        CreateMap<CategoryModel, CategoryDto>();
    }
}
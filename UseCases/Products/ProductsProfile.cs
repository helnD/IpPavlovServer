using AutoMapper;
using Domain;
using UseCases.Products.GetProducts.Dtos;

namespace UseCases.Products;

/// <summary>
/// Mapper profile for products.
/// </summary>
public class ProductsProfile : Profile
{
    public ProductsProfile()
    {
        CreateMap<Partner, PartnerDto>();
        CreateMap<Category, CategoryDto>();
        CreateMap<Product, ProductDto>();
    }
}
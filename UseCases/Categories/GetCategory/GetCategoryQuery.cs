using System.ComponentModel.DataAnnotations;
using MediatR;
using UseCases.Products.GetProducts.Dtos;

namespace UseCases.Categories.GetCategory;

/// <summary>
/// Query for category.
/// </summary>
public class GetCategoryQuery : IRequest<CategoryDto>
{
    /// <summary>
    /// Category route name.
    /// </summary>
    [Required]
    public string RouteName { get; init; }
}
using System.ComponentModel.DataAnnotations;
using MediatR;
using UseCases.Products.GetProducts.Dtos;

namespace UseCases.Products.GetProductById
{
    /// <summary>
    /// Get product by id.
    /// </summary>
    public class GetProductByIdQuery : IRequest<ProductDto>
    {
        [Required]
        public int ProductId { get; init; }
    }
}
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCases.Products.GetProductById;
using UseCases.Products.GetProducts;
using UseCases.Products.GetProducts.Dtos;

namespace WebApplication.Controllers.Api;

/// <summary>
/// API Controller for products.
/// </summary>
[ApiController]
[Route("api/v1/products")]
[AllowAnonymous]
public class ProductsApiController
{
    private readonly IMediator _mediator;

    public ProductsApiController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get products by specific name, category and producer.
    /// </summary>
    /// <param name="query">Get products query.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Page with products.</returns>
    [HttpGet]
    public async Task<Page<ProductDto>> GetProducts([FromQuery]GetProductsQuery query, CancellationToken cancellationToken)
    {
        return await _mediator.Send(query, cancellationToken);
    }

    /// <summary>
    /// Get product by id.
    /// </summary>
    /// <param name="query">Get products query.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Required product.</returns>
    [HttpGet("{ProductId}")]
    public async Task<ProductDto> GetProductById([FromRoute]GetProductByIdQuery query,
        CancellationToken cancellationToken)
    {
        return await _mediator.Send(query, cancellationToken);
    }
}
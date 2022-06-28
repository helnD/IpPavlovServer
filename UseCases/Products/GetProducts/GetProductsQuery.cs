using Infrastructure.Pagination;
using UseCases.Products.GetProducts.Dtos;

namespace UseCases.Products.GetProducts;

/// <summary>
/// Query for products by specific name, category and producer.
/// </summary>
public class GetProductsQuery : PagedRequest<ProductDto>
{
    /// <summary>
    /// Product name.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Partners.
    /// </summary>
    public string ProducerIds { get; init; }

    /// <summary>
    /// Categories.
    /// </summary>
    public string CategoryIds { get; init; }
}
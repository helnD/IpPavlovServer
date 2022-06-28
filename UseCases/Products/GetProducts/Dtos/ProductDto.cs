using Domain;

namespace UseCases.Products.GetProducts.Dtos;

/// <summary>
/// Data Transfer Object for product.
/// </summary>
public class ProductDto
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Number of product position in excel document.
    /// </summary>
    public int DocumentId { get; init; }

    /// <summary>
    /// Product code.
    /// </summary>
    public int Code { get; init; }

    /// <summary>
    /// Product description.
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Quantity of products.
    /// </summary>
    public int Quantity { get; init; }

    /// <summary>
    /// Unit of measurement.
    /// </summary>
    public string Unit { get; init; }

    /// <summary>
    /// Price of product.
    /// </summary>
    public decimal Price { get; init; }

    /// <summary>
    /// Category of product.
    /// </summary>
    public CategoryDto Category { get; init; }

    /// <summary>
    /// Partner that supply product.
    /// </summary>
    public PartnerDto Producer { get; init; }

    /// <summary>
    /// Image of product.
    /// </summary>
    public Image Image { get; init; }
}
using Domain;

namespace UseCases.Products.GetProducts.Dtos;

/// <summary>
/// Data Transfer Object for product.
/// </summary>
public class PartnerDto
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Partner name.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Partner description.
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Link to site of partner.
    /// </summary>
    public string Link { get; init; }

    /// <summary>
    /// Logo of partner.
    /// </summary>
    public Image Image { get; init; }
}
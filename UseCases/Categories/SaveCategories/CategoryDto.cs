using UseCases.Certificates.SaveCertificates;

namespace UseCases.Categories.SaveCategories;

public class CategoryDto
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Category name.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Image of certificate.
    /// </summary>
    public ImageDto Icon { get; init; }
}
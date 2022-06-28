namespace Infrastructure.Settings;

/// <summary>
/// Images settings.
/// </summary>
public class ImagesSettings
{
    /// <summary>
    /// Folder for all images.
    /// </summary>
    public string Root { get; init; }

    /// <summary>
    /// Subfolder for categories.
    /// </summary>
    public string Categories { get; init; }

    /// <summary>
    /// Subfolder for products.
    /// </summary>
    public string Products { get; init; }

    /// <summary>
    /// Subfolder for partners.
    /// </summary>
    public string Partners { get; init; }

    /// <summary>
    /// Subfolder for certificates.
    /// </summary>
    public string Certificates { get; init; }

    /// <summary>
    /// Prefix for mini image.
    /// </summary>
    public string MiniPrefix { get; init; }
}
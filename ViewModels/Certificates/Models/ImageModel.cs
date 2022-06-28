using ViewModels.Common;

namespace ViewModels.Certificates.Models;

public class ImageModel : Notifier
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name and extension of file.
    /// </summary>
    public string Name => System.IO.Path.GetFileName(Path);

    /// <summary>
    /// Path to image.
    /// </summary>
    public string Path { get; set; }

    public bool IsUpdated { get; set; }
}
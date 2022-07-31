using System.IO;
using MediatR;

namespace UseCases.Resources.UploadImage;

/// <summary>
/// Command for image uploading.
/// </summary>
public class UploadImageCommand : IRequest<int>
{
    /// <summary>
    /// Image name.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Resource type.
    /// </summary>
    public string Type { get; init; }

    /// <summary>
    /// File stream.
    /// </summary>
    public Stream ImageStream { get; init; }
}
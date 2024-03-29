﻿using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;

namespace Infrastructure.Abstractions;

/// <summary>
/// Interface for resize images.
/// </summary>
public interface IImageResizer
{
    /// <summary>
    /// Reduce image to threshold.
    /// </summary>
    /// <param name="image">Image to reduce.</param>
    /// <param name="threshold">Size threshold.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<Stream> Reduce(FileStream image, int threshold, CancellationToken cancellationToken);

    /// <summary>
    /// Reduce image to threshold.
    /// </summary>
    /// <param name="image">Image to reduce.</param>
    /// <param name="threshold">Size threshold.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<Image> Reduce(Image image, int threshold, CancellationToken cancellationToken);
}
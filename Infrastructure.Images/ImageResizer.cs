using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Infrastructure.Images;

/// <summary>
/// Class for resizing images using ImageSharp.
/// </summary>
public class ImageResizer : IImageResizer
{
    /// <inheritdoc/>
    public async Task<Stream> Reduce(FileStream image, int threshold, CancellationToken cancellationToken)
    {
        image.Position = 0;
        using var imageToResize = await Image.LoadAsync(image);

        var height = imageToResize.Height;
        var width = imageToResize.Width;

        var newHeight = height > width ? threshold : height * threshold / width;
        var newWidth = height < width ? threshold : width * threshold / height;

        imageToResize.Mutate(img => img.Resize(newWidth, newHeight));

        image.Position = 0;
        var format = await Image.DetectFormatAsync(image, cancellationToken);

        var resizedImage = new MemoryStream();
        await imageToResize.SaveAsync(resizedImage, format, cancellationToken);

        resizedImage.Position = 0;
        return resizedImage;
    }

    public Task<Image> Reduce(Image image, int threshold, CancellationToken cancellationToken)
    {
        var height = image.Height;
        var width = image.Width;

        var newHeight = height > width ? threshold : height * threshold / width;
        var newWidth = height < width ? threshold : width * threshold / height;

        var reducedImage = image.Clone(img => img.Resize(newWidth, newHeight));

        return Task.FromResult(reducedImage);
    }
}
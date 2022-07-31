using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Abstractions;
using Infrastructure.Exceptions;
using Infrastructure.Settings;
using MediatR;
using Microsoft.Extensions.Options;

namespace UseCases.Resources.UploadImage;

/// <summary>
/// Handler for image uploading.
/// </summary>
public class UploadImageHandler : IRequestHandler<UploadImageCommand, int>
{
    private readonly ImagesSettings _imagesSettings;
    private readonly IImageResizer _imageResizer;
    private readonly IDbContext _context;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="imagesSettings">Image settings.</param>
    /// <param name="context">Database context.</param>
    /// <param name="imageResizer">Image resizer.</param>
    public UploadImageHandler(IOptions<ImagesSettings> imagesSettings,
        IDbContext context,
        IImageResizer imageResizer)
    {
        _imagesSettings = imagesSettings.Value;
        _context = context;
        _imageResizer = imageResizer;
    }

    /// <summary>
    /// Handle image uploading.
    /// </summary>
    /// <param name="request">Upload image request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task<int> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        var useCasesRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        var nameWithoutExtension = Path.GetFileNameWithoutExtension(request.Name);
        var invalidChars = Path.GetInvalidFileNameChars();

        var validFileName = new string(nameWithoutExtension
            .Where(symbol => !invalidChars.Contains(symbol))
            .ToArray());

        var imageFolder = Path.Combine(useCasesRoot, GetImageFolder(request.Type));
        var imagesNumberWithSameName = new DirectoryInfo(imageFolder)
            .GetFiles()
            .Count(image => image.Name.Contains(validFileName));

        var fileExtenstion = Path.GetExtension(request.Name);
        var imageName = imagesNumberWithSameName == 0
            ? validFileName + fileExtenstion
            : $"{validFileName}-{imagesNumberWithSameName + 1}{fileExtenstion}";

        var imagePath = Path.Combine(imageFolder, imageName);
        await using var imageFile = new FileStream(imagePath, FileMode.Create);
        await request.ImageStream.CopyToAsync(imageFile, cancellationToken);

        await HandleMini(request.Type, imageFolder, imageName, imageFile, cancellationToken);

        var image = new Image {Path = imagePath};
        await _context.Images.AddAsync(image, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return image.Id;
    }

    private async Task HandleMini(string type, string folder, string name, FileStream image,
        CancellationToken cancellationToken)
    {
        if (type != "Certificates" && type != "Products")
        {
            return;
        }

        var filename = Path.GetFileNameWithoutExtension(name);
        var extension = Path.GetExtension(name);

        var miniFilename = _imagesSettings.MiniPrefix + filename + extension;
        var miniFullname = Path.Combine(folder, miniFilename);

        var miniImage = await _imageResizer.Reduce(image, 512, cancellationToken);
        await using var miniFile = File.Create(miniFullname, (int) miniImage.Length, FileOptions.Asynchronous);
        miniImage.Position = 0;
        await miniImage.CopyToAsync(miniFile, cancellationToken);
    }

    private string GetImageFolder(string type)
    {
        var foldersFields = typeof(ImagesSettings).GetProperties();
        var currentFolderField = foldersFields
            .FirstOrDefault(field => String.Equals(field.Name, type, StringComparison.CurrentCultureIgnoreCase));

        if (currentFolderField == default)
        {
            throw new NotFoundException($"Image type {type} not found.");
        }

        var currentFolder = (string) currentFolderField.GetValue(_imagesSettings);
        return Path.Combine(_imagesSettings.Root, currentFolder);
    }
}
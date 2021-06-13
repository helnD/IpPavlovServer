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

namespace UseCases.Resources.UploadImage
{
    /// <summary>
    /// Handler for image uploading.
    /// </summary>
    public class UploadImageHandler : IRequestHandler<UploadImageCommand, int>
    {
        private readonly ImagesSettings _imagesSettings;
        private readonly IDbContext _context;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="imagesSettings">Image settings.</param>
        /// <param name="context">Database context.</param>
        public UploadImageHandler(IOptions<ImagesSettings> imagesSettings, IDbContext context)
        {
            _imagesSettings = imagesSettings.Value;
            _context = context;
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
            var imageFolder = Path.Combine(useCasesRoot, GetImageFolder(request.Type));
            var imagesNumberWithSameName = new DirectoryInfo(imageFolder)
                .GetFiles()
                .Count(image => image.Name.Contains(nameWithoutExtension));

            var fileExtenstion = Path.GetExtension(request.Name);
            var imageName = imagesNumberWithSameName == 0
                ? nameWithoutExtension + fileExtenstion
                : $"{nameWithoutExtension}-{imagesNumberWithSameName + 1}{fileExtenstion}";

            var imagePath = Path.Combine(useCasesRoot, imageFolder, imageName);
            await using var imageFile = File.Create(imagePath, (int)request.ImageStream.Length, FileOptions.Asynchronous);
            await request.ImageStream.CopyToAsync(imageFile, cancellationToken);

            var image = new Image { Path = imagePath };
            await _context.Images.AddAsync(image, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return image.Id;
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

            var currentFolder = (string)currentFolderField.GetValue(_imagesSettings);
            return Path.Combine(_imagesSettings.Root, currentFolder);
        }
    }
}
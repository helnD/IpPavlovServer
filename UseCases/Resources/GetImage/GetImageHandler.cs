using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Abstractions;
using Infrastructure.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace UseCases.Resources.GetImage;

/// <summary>
/// Handle query for image.
/// </summary>
public class GetImageHandler : IRequestHandler<GetImageQuery, FileStream>
{
    private readonly IDbContext _context;
    private readonly ImagesSettings _imagesSettings;
    private readonly Locker _locker;

    public GetImageHandler(IDbContext context, IOptions<ImagesSettings> imagesSettings, Locker locker)
    {
        _context = context;
        _locker = locker;
        _imagesSettings = imagesSettings.Value;
    }

    /// <summary>
    /// Handler of query for image.
    /// </summary>
    /// <param name="request">Get image request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task<FileStream> Handle(GetImageQuery request, CancellationToken cancellationToken)
    {
        var requiredImage = await _context.Images
            .FirstOrDefaultAsync(image => image.Id == request.ImageId, cancellationToken);

        if (requiredImage is null)
        {
            throw new ArgumentException($"Image with id {request.ImageId} not found");
        }

        var fileName = requiredImage.Path;

        var isImageProductOrPartner = requiredImage.Path.Contains("Products") ||
                                      requiredImage.Path.Contains("Partners");
        if (request.Size.ToUpper() == "MINI" && isImageProductOrPartner)
        {
            var folderPath = Path.GetDirectoryName(requiredImage.Path);
            fileName = Path.Combine(folderPath, _imagesSettings.MiniPrefix + requiredImage.Name);
        }

        lock (_locker)
        {
            return new FileStream(fileName, FileMode.Open, FileAccess.Read);
        }
    }
}
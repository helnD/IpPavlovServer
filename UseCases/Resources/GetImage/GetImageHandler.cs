using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Infrastructure.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace UseCases.Resources.GetImage
{
    public class GetImageHandler : IRequestHandler<GetImageQuery, FileStream>
    {
        private readonly ImagesSettings _imagesSettings;
        private readonly IDbContext _context;

        public GetImageHandler(IOptions<ImagesSettings> imagesSettings, IDbContext context)
        {
            _context = context;
            _imagesSettings = imagesSettings.Value;
        }

        public async Task<FileStream> Handle(GetImageQuery request, CancellationToken cancellationToken)
        {
            var requiredImage = await _context.Images
                .FirstOrDefaultAsync(image => image.Id == request.Id, cancellationToken);

            if (requiredImage is null)
            {
                throw new ArgumentException($"Image with id {request.Id} not found");
            }

            var filepath = GetFilePath(request.Type, requiredImage.Name);
            return new FileStream(filepath, FileMode.Open, FileAccess.Read);
        }

        private string GetFilePath(string type, string name)
        {
            var imagesSettingsType = typeof(ImagesSettings);
            var typeFolder = imagesSettingsType.GetProperties()
                .FirstOrDefault(property => property.Name == type);

            if (typeFolder == null)
            {
                throw new ArgumentOutOfRangeException($"Type {type} not found");
            }

            var path = Path.Combine(_imagesSettings.Root, type, name);
            return path;
        }
    }
}
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UseCases.Resources.GetImage
{
    /// <summary>
    /// Handle query for image.
    /// </summary>
    public class GetImageHandler : IRequestHandler<GetImageQuery, FileStream>
    {
        private readonly IDbContext _context;

        public GetImageHandler(IDbContext context)
        {
            _context = context;
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

            return new FileStream(requiredImage.Path, FileMode.Open, FileAccess.Read);
        }
    }
}
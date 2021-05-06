using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCases.Resources.GetImage;

namespace WebApplication.Controllers
{
    /// <summary>
    /// Controller for images uploading/downloading
    /// </summary>
    [ApiController]
    [Route("api/v1/images")]
    public class ImagesApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImagesApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Download image by id and image type.
        /// </summary>
        [HttpGet("{imageId}")]
        public async Task<IActionResult> DownloadImage(int imageId, string type, CancellationToken cancellationToken)
        {
            var query = new GetImageQuery
            {
                Id = imageId,
                Type = type
            };

            var image = await _mediator.Send(query, cancellationToken);
            var extension = image.Name.Split('.').Last();
            return File(image, GetMimeType(extension));
        }

        private string GetMimeType(string extension) => extension switch
        {
            "svg" => "image/svg+xml",
            "png" => "image/png",
            "jpg" => "image/jpeg",
            _ => throw new ArgumentOutOfRangeException("Not supported media type")
        };
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UseCases.Resources.GetImage;
using UseCases.Resources.UploadImage;

namespace WebApplication.Controllers.Api
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
        [HttpGet("{imageId}/{size=normal}")]
        public async Task<IActionResult> DownloadImage([FromRoute]GetImageQuery query, CancellationToken cancellationToken)
        {
            var image = await _mediator.Send(query, cancellationToken);
            var extension = image.Name.Split('.').Last();
            return File(image, GetMimeType(extension));
        }

        /// <summary>
        /// Upload new image to the system.
        /// </summary>
        /// <param name="image">Image file.</param>
        /// <param name="type">Image type (folder).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [HttpPost]
        public async Task<int> UploadImage([Required][FromForm]IFormFile image, [Required][FromQuery]string type,
            CancellationToken cancellationToken)
        {
            var command = new UploadImageCommand
            {
                Name = image.FileName,
                Type = type,
                ImageStream = image.OpenReadStream()
            };
            return await _mediator.Send(command, cancellationToken);
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
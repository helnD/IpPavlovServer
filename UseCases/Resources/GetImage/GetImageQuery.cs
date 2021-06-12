using System.ComponentModel.DataAnnotations;
using System.IO;
using MediatR;

namespace UseCases.Resources.GetImage
{
    /// <summary>
    /// Returns image by id and type.
    /// </summary>
    public class GetImageQuery : IRequest<FileStream>
    {
        /// <summary>
        /// Id of image.
        /// </summary>
        [Required]
        public int ImageId { get; init; }

        /// <summary>
        /// Size type of image.
        /// </summary>
        public string Size { get; init; } = "normal";
    }
}
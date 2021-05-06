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
        /// Name of directory with images.
        /// </summary>
        /// <example>Categories</example>
        /// <example>Products</example>
        [Required]
        public string Type { get; init; } = "Products";

        [Required]
        public int Id { get; init; }
    }
}
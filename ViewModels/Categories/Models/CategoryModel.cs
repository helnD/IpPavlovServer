using ViewModels.Certificates.Models;
using ViewModels.Common;

namespace ViewModels.Categories.Models
{
    public class CategoryModel : Notifier
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Category name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Image of certificate.
        /// </summary>
        public ImageModel Icon { get; set; } = new();
    }
}
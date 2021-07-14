using ViewModels.Certificates.Models;
using ViewModels.Common;

namespace ViewModels.Partners.Models
{
    public class PartnerModel : Notifier
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Partner name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Partner description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Logo of partner.
        /// </summary>
        public ImageModel Image { get; set; } = new();
    }
}
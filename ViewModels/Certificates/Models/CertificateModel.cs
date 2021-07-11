using ViewModels.Common;

namespace ViewModels.Certificates.Models
{
    public class CertificateModel : Notifier
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Certificate description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Image of certificate.
        /// </summary>
        public ImageModel Image { get; set; } = new();
    }
}
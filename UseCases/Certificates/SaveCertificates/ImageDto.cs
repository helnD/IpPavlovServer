namespace UseCases.Certificates.SaveCertificates
{
    /// <summary>
    /// Image DTO.
    /// </summary>
    public class ImageDto
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Path to image.
        /// </summary>
        public string Path { get; init; }

        public bool IsUpdated { get; init; }
    }
}
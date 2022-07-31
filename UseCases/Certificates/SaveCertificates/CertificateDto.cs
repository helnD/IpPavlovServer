namespace UseCases.Certificates.SaveCertificates;

/// <summary>
/// Certificate DTO.
/// </summary>
public class CertificateDto
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Certificate description.
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Image of certificate.
    /// </summary>
    public ImageDto Image { get; init; }
}
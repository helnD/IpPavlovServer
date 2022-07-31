using UseCases.Certificates.SaveCertificates;

namespace UseCases.Partners.SavePartnersCommand;

public class PartnerDto
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Partner name.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Partner description.
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Logo of partner.
    /// </summary>
    public ImageDto Image { get; init; }
}
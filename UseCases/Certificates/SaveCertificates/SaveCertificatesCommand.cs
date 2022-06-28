using System.Collections.Generic;
using MediatR;

namespace UseCases.Certificates.SaveCertificates;

/// <summary>
/// Saves/Edits certificates.
/// </summary>
public class SaveCertificatesCommand : IRequest
{
    /// <summary>
    /// Added certificates.
    /// </summary>
    public IEnumerable<CertificateDto> Added { get; init; }

    /// <summary>
    /// Removed certificates.
    /// </summary>
    public IEnumerable<CertificateDto> Removed { get; init; }

    /// <summary>
    /// Updated certificates.
    /// </summary>
    public IEnumerable<CertificateDto> Updated { get; init; }
}
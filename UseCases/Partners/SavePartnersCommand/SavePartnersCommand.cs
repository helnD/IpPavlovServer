using System.Collections.Generic;
using MediatR;

namespace UseCases.Partners.SavePartnersCommand;

public class SavePartnersCommand : IRequest
{
    /// <summary>
    /// Added certificates.
    /// </summary>
    public IEnumerable<PartnerDto> Added { get; init; }

    /// <summary>
    /// Removed certificates.
    /// </summary>
    public IEnumerable<PartnerDto> Removed { get; init; }

    /// <summary>
    /// Updated certificates.
    /// </summary>
    public IEnumerable<PartnerDto> Updated { get; init; }
}
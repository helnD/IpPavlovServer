using System.Collections.Generic;
using Domain;
using MediatR;

namespace UseCases.Partners.GetPartners
{
    /// <summary>
    /// Get all partners.
    /// </summary>
    public class GetPartnersQuery : IRequest<IEnumerable<Partner>>
    {

    }
}
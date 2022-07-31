using System.Collections.Generic;
using Domain;
using MediatR;

namespace UseCases.SalesRepresentatives.GetRepresentatives;

/// <summary>
/// Get all sales representatives query.
/// </summary>
public class GetRepresentativesQuery : IRequest<IEnumerable<SalesRepresentative>>
{

}
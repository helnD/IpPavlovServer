using System.Collections.Generic;
using Domain;
using MediatR;

namespace UseCases.Leaders.GetLeaders;

/// <summary>
/// Get all leaders of sales.
/// </summary>
public class GetLeadersQuery : IRequest<IList<SalesLeader>>
{

}
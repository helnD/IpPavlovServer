using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCases.SalesRepresentatives.GetRepresentatives;

namespace WebApplication.Controllers
{
    /// <summary>
    /// API controller for sales representatives.
    /// </summary>
    [ApiController]
    [Route("api/v1/representatives")]
    public class SalesRepresentativesApiController
    {
        private readonly IMediator _mediator;

        public SalesRepresentativesApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all sales representatives.
        /// </summary>
        /// <param name="query">Get sales representatives query.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [HttpGet]
        public async Task<IEnumerable<SalesRepresentative>> GetRepresentatives([FromQuery] GetRepresentativesQuery query,
            CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }
    }
}
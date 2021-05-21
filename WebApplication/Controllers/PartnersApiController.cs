using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCases.Partners.GetPartners;

namespace WebApplication.Controllers
{
    /// <summary>
    /// API controller for partners.
    /// </summary>
    [ApiController]
    [Route("api/v1/partners")]
    public class PartnersApiController
    {
        private readonly IMediator _mediator;

        public PartnersApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all partners.
        /// </summary>
        /// <param name="query">Get all partners query.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [HttpGet]
        public async Task<IEnumerable<Partner>> GetAllPartners([FromQuery]GetPartnersQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }
    }
}
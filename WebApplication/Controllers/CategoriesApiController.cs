using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCases.Categories.GetCategories;

namespace WebApplication.Controllers
{
    /// <summary>
    /// Controller for categories.
    /// </summary>
    [ApiController]
    [Route("api/v1/categories")]
    public class CategoriesApiController
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mediator">MediatR object.</param>
        public CategoriesApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns all categories.
        /// </summary>
        /// <param name="query"><see cref="GetAllCategoriesQuery"/> request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task<IEnumerable<Category>> GetAllCategories([FromQuery]GetAllCategoriesQuery query, CancellationToken cancellationToken)
        {
            return await _mediator.Send(query, cancellationToken);
        }
    }
}
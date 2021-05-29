using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCases.Questions.AddQuestion;

namespace WebApplication.Controllers
{
    /// <summary>
    /// Controller for questions.
    /// </summary>
    [ApiController]
    [Route("api/v1/questions")]
    public class QuestionsApiController
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mediator">MediatR object.</param>
        public QuestionsApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Add new question.
        /// </summary>
        /// <param name="command">Add new question command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [HttpPost]
        public async Task AddQuestion([FromBody]AddQuestionCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
        }
    }
}
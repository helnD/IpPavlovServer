using System.Threading;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace UseCases.Questions.AddQuestion
{
    /// <summary>
    /// Handle command for question asking.
    /// </summary>
    public class AddQuestionHandler : AsyncRequestHandler<AddQuestionCommand>
    {
        private readonly IEmailSender _emailSender;
        private readonly IDbContext _context;
        private readonly ILogger<AddQuestionHandler> _logger;

        public AddQuestionHandler(IEmailSender emailSender, IDbContext context, ILogger<AddQuestionHandler> logger)
        {
            _emailSender = emailSender;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Handler of command for question asking.
        /// </summary>
        /// <param name="request"><see cref="AddQuestionCommand"/> request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        protected override async Task Handle(AddQuestionCommand request, CancellationToken cancellationToken)
        {
            var newQuestion = new Question
            {
                Owner = request.Owner,
                OwnerEmail = request.OwnerEmail,
                QuestionText = request.QuestionText
            };

            await _context.Questions.AddAsync(newQuestion, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _emailSender.SendQuestionAsync(request.Owner, request.OwnerEmail, request.QuestionText,
                cancellationToken);

            _logger.LogInformation("Question from {Owner} was asked", request.Owner);
        }
    }
}
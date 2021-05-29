using System.ComponentModel.DataAnnotations;
using MediatR;

namespace UseCases.Questions.AddQuestion
{
    /// <summary>
    /// Command for add new question.
    /// </summary>
    public class AddQuestionCommand : IRequest
    {
        /// <summary>
        /// Name of user who ask a question.
        /// </summary>
        [Required]
        public string Owner { get; init; }

        /// <summary>
        /// Email of owner for feedback.
        /// </summary>
        [Required]
        public string OwnerEmail { get; init; }

        /// <summary>
        /// Question text.
        /// </summary>
        [Required]
        public string QuestionText { get; init; }
    }
}
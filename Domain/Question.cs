using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    /// <summary>
    /// Question from site.
    /// </summary>
    public record Question
    {
        /// <summary>
        /// Question id.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Name of user who ask a question.
        /// </summary>
        public string Owner { get; init; }

        /// <summary>
        /// Email of owner for feedback.
        /// </summary>
        public string OwnerEmail { get; init; }

        /// <summary>
        /// Question text.
        /// </summary>
        public string QuestionText { get; init; }

        /// <summary>
        /// Date and time when email was send.
        /// </summary>
        [Column(TypeName = "timestamptz")]
        public DateTime SendingTime { get; init; } = DateTime.Now;
    }
}
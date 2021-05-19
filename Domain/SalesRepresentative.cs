using System;

namespace Domain
{
    /// <summary>
    /// Our sales representative.
    /// </summary>
    public record SalesRepresentative
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Region where sales representative works.
        /// </summary>
        public string Region { get; init; }

        /// <summary>
        /// Firstname.
        /// </summary>
        public string FirstName { get; init; }

        /// <summary>
        /// Lastname.
        /// </summary>
        public string LastName { get; init; }

        /// <summary>
        /// Middle name.
        /// </summary>
        public string MiddleName { get; init; }

        /// <summary>
        /// Phone number.
        /// </summary>
        public string Phone { get; init; }

        /// <summary>
        /// Time when representative begins work.
        /// </summary>
        public TimeSpan StartOfWork { get; init; }

        /// <summary>
        /// Time when representative ends work.
        /// </summary>
        public TimeSpan EndOfWork { get; init; }
    }
}
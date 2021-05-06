using System;

namespace Domain
{
    public record SalesRepresentative
    {
        public int Id { get; init; }

        public string Region { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string MiddleName { get; init; }

        public string Phone { get; init; }

        public TimeSpan StartOfWork { get; init; }

        public TimeSpan EndOfWork { get; init; }
    }
}
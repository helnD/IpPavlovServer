using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Infrastructure.Pagination
{
    /// <summary>
    /// Base class for queries that returns content as page.
    /// </summary>
    public abstract class PagedRequest<T> : IRequest<Page<T>> where T : new()
    {
        /// <summary>
        /// Pagination number.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int PageNumber { get; init; } = 1;

        /// <summary>
        /// Page size.
        /// </summary>
        [Range(1, 60)]
        public int PageSize { get; init; } = 10;
    }
}
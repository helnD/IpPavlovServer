using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Pagination
{
    /// <summary>
    /// Class for represent page (needed for pagination).
    /// </summary>
    /// <typeparam name="T">Type of page content</typeparam>
    public class Page<T> where T : new()
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="source">Data source.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="mapping">Mapping logic.</param>
        public static async Task<Page<T>> CreatePageAsync(IQueryable<object> source, int pageNumber, int pageSize,
            CancellationToken cancellationToken, Func<object, T> mapping = null)
        {
            var mappingLogic = mapping ?? (obj => (T) obj);

                var sourceItemsCount = source.Count();
            var total = sourceItemsCount / pageSize + (sourceItemsCount % pageSize != 0 ? 1 : 0);

            var content = await source.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(item => mappingLogic(item))
                .ToListAsync(cancellationToken);

            return new Page<T>
            {
                Total = total,
                Content = content,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        /// <summary>
        /// Page content.
        /// </summary>
        public IList<T> Content { get; init; }

        /// <summary>
        /// Total number of pages.
        /// </summary>
        public int Total { get; init; }

        /// <summary>
        /// Current page.
        /// </summary>
        public int PageNumber { get; init; }

        /// <summary>
        /// Size of page.
        /// </summary>
        public int PageSize { get; init; }
    }
}
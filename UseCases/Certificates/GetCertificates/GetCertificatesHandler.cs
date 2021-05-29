using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UseCases.Certificates.GetCertificates
{
    /// <summary>
    /// Handle query for certificates.
    /// </summary>
    public class GetCertificatesHandler : IRequestHandler<GetCertificatesQuery, IEnumerable<Certificate>>
    {
        private readonly IDbContext _context;

        public GetCertificatesHandler(IDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handler of query for all certificates.
        /// </summary>
        /// <param name="request">Get certificates query.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task<IEnumerable<Certificate>> Handle(GetCertificatesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Certificates.Include(certificate => certificate.Image)
                .ToListAsync(cancellationToken);
        }
    }
}
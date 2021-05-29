using System.Collections.Generic;
using Domain;
using MediatR;

namespace UseCases.Certificates.GetCertificates
{
    /// <summary>
    /// Query for all certificates.
    /// </summary>
    public class GetCertificatesQuery : IRequest<IEnumerable<Certificate>>
    {

    }
}
using System.IO;
using MediatR;

namespace UseCases.Resources.GetPriceList
{
    /// <summary>
    /// Returns stream with price list.
    /// </summary>
    public class GetPriceListQuery : IRequest<Stream>
    {

    }
}
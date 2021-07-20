using System.Collections.Generic;
using Domain;
using MediatR;

namespace UseCases.Stocks.GetAllStocks
{
    public class GetAllStocksQuery : IRequest<IEnumerable<Stock>>
    {

    }
}
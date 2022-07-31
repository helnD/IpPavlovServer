using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Settings;
using MediatR;
using Microsoft.Extensions.Options;

namespace UseCases.Resources.GetPriceList;

public class GetPriceListHandler : IRequestHandler<GetPriceListQuery, Stream>
{
    private readonly FilesSettings _filesSettings;

    public GetPriceListHandler(IOptions<FilesSettings> filesSettings)
    {
        _filesSettings = filesSettings.Value;
    }

    public async Task<Stream> Handle(GetPriceListQuery request, CancellationToken cancellationToken)
    {
        var filename = Path.Combine(_filesSettings.Root, _filesSettings.PriceList);
        var file = new FileStream(filename, FileMode.Open, FileAccess.Read);
        return file;
    }
}
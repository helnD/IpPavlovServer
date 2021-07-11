using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Abstractions
{
    public interface IImageApi
    {
        public Task<int> UploadImageAsync(string pathToImage, string type, CancellationToken cancellationToken);
    }
}
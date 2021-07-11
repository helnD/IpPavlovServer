using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using RestSharp;

namespace Editor.Desktop.Services
{
    public class ImageApi : IImageApi
    {
        private readonly RestClient _restClient;

        public ImageApi()
        {
            _restClient = new RestClient("http://localhost:5000/api/v1/images");
        }

        public async Task<int> UploadImageAsync(string pathToImage, string type, CancellationToken cancellationToken)
        {
            var restRequest = new RestRequest();
            restRequest.AddFile("image", pathToImage);
            restRequest.AddQueryParameter("type", type);

            var response = await _restClient.ExecutePostAsync(restRequest, cancellationToken);
            return int.Parse(response.Content);
        }
    }
}
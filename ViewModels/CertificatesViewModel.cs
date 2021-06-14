using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using UseCases.Certificates.GetCertificates;

namespace ViewModels
{
    /// <summary>
    /// View model for certificates.
    /// </summary>
    public class CertificatesViewModel : Notifier
    {
        private readonly IMediator _mediator;

        private ObservableCollection<Certificate> _certificates;
        private Certificate _selectedCertificate = null;
        private string _imageUri = "init";

        public CertificatesViewModel(IMediator mediator, InvokeAsynchronously initContext)
        {
            _mediator = mediator;

            initContext(async () => await InitializeCertificates(default));
        }

        public ObservableCollection<Certificate> Certificates
        {
            get => _certificates;
            set
            {
                _certificates = value;
                OnPropertyChanged();
            }
        }

        public Certificate SelectedCertificate
        {
            get => _selectedCertificate;
            set
            {
                DownloadImage(value.Image.Id);
                _selectedCertificate = value;
            }
        }

        public string ImageUri
        {
            get => _imageUri;
            set
            {
                _imageUri = value;
                OnPropertyChanged();
            }
        }

        private async Task InitializeCertificates(CancellationToken cancellationToken)
        {
            var certificates = await _mediator.Send(new GetCertificatesQuery(), cancellationToken);
            Certificates = new ObservableCollection<Certificate>(certificates);
        }

        private void DownloadImage(int imageId)
        {
            ImageUri = $"http://localhost:5000/api/v1/images/{imageId}";;
        }
    }
}
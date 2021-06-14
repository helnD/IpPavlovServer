using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
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

        private async Task InitializeCertificates(CancellationToken cancellationToken)
        {
            var certificates = await _mediator.Send(new GetCertificatesQuery(), cancellationToken);
            Certificates = new ObservableCollection<Certificate>(certificates);
        }
    }
}
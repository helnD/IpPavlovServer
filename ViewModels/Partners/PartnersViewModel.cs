using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using MediatR;
using UseCases.Certificates.SaveCertificates;
using UseCases.Partners.GetPartners;
using ViewModels.Certificates.Models;
using ViewModels.Common;
using ViewModels.Common.ViewModel;
using ViewModels.Partners.Models;

namespace ViewModels.Partners
{
    public class PartnersViewModel : EditableTableViewModel<PartnersModel, PartnerModel>
    {
        private readonly IMediator _mediator;
        private readonly InvokeAsynchronously _initContext;
        private readonly IMapper _mapper;
        private readonly IFileDialog _fileDialog;

        private PartnerModel _selectedCertificate;

        public PartnersViewModel(IMediator mediator, InvokeAsynchronously initContext, IMapper mapper, IFileDialog fileDialog)
        {
            _mediator = mediator;
            _initContext = initContext;
            _mapper = mapper;
            _fileDialog = fileDialog;
            Model = new PartnersModel();

            ChooseImageCommand = new RelayCommand<ImageModel>(ChooseFile);
        }

        public RelayCommand<ImageModel> ChooseImageCommand { get; }

        public PartnerModel SelectedCertificate
        {
            get => _selectedCertificate;
            set
            {
                if (value is not null)
                {
                    SetImage(value.Image);
                }

                _selectedCertificate = value;
            }
        }

        public string ImageUri { get; set; } = "init";

        private async Task InitializeCertificates(CancellationToken cancellationToken)
        {
            var certificates = await _mediator.Send(new GetPartnersQuery(), cancellationToken);
            var mappedCertificates = _mapper.Map<IEnumerable<PartnerModel>>(certificates);
            Model = new PartnersModel
            {
                TableContent = new ObservableCollection<PartnerModel>(mappedCertificates)
            };
            ImageUri = null;
        }

        public override async Task LoadAsync()
        {
            await _initContext(async () => await InitializeCertificates(default));
        }

        protected override async Task Save()
        {
            var command = _mapper.Map<SaveCertificatesCommand>(Model);
            await _mediator.Send(command);
            await _initContext(async () => await InitializeCertificates(default));
        }

        protected override async Task UndoChanges()
        {
            await _initContext(async () => await InitializeCertificates(default));
        }

        private void SetImage(ImageModel image)
        {
            ImageUri = image switch
            {
                {Id: var id} when id != default => $"http://localhost:5000/api/v1/images/{id}",
                {Path: var path} when path != default => path,
                _ => null
            };
        }

        private void ChooseFile(ImageModel model)
        {
            var fileName = _fileDialog.ShowFileDialog();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            model.Id = default;
            model.Path = fileName;
            model.IsUpdated = true;
            ImageUri = fileName;
        }
    }
}
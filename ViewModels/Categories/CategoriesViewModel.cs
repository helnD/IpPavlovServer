using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using MediatR;
using UseCases.Categories.GetCategories;
using UseCases.Categories.SaveCategories;
using UseCases.Certificates.SaveCertificates;
using ViewModels.Categories.Models;
using ViewModels.Certificates.Models;
using ViewModels.Common;
using ViewModels.Common.ViewModel;

namespace ViewModels.Categories
{
    /// <summary>
    /// View model for certificates.
    /// </summary>
    public class CategoriesViewModel : EditableTableViewModel<CategoriesModel, CategoryModel>
    {
        private readonly IMediator _mediator;
        private readonly InvokeAsynchronously _initContext;
        private readonly IMapper _mapper;
        private readonly IFileDialog _fileDialog;

        private CategoryModel _selectedCategory;

        public CategoriesViewModel(IMediator mediator, InvokeAsynchronously initContext, IMapper mapper, IFileDialog fileDialog)
        {
            _mediator = mediator;
            _initContext = initContext;
            _mapper = mapper;
            _fileDialog = fileDialog;
            Model = new CategoriesModel();

            ChooseImageCommand = new RelayCommand<ImageModel>(ChooseFile);
        }

        public string ImageUri { get; set; }

        public CategoryModel SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (value is not null)
                {
                    SetImage(value.Icon);
                }

                _selectedCategory = value;
            }
        }

        public RelayCommand<ImageModel> ChooseImageCommand { get; }

        private async Task InitializeCertificates(CancellationToken cancellationToken)
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
            var mappedCategories = _mapper.Map<IEnumerable<CategoryModel>>(categories);
            Model = new CategoriesModel
            {
                TableContent = new ObservableCollection<CategoryModel>(mappedCategories)
            };
            ImageUri = null;
        }

        public override async Task LoadAsync()
        {
            await InitializeCertificates(default);
        }

        protected override async Task Save()
        {
            var command = _mapper.Map<SaveCategoriesCommand>(Model);
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
            ImageUri = fileName;
        }
    }
}
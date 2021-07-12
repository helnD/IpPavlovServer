using ViewModels.Categories;

namespace Editor.Desktop.Views.Common
{
    public partial class Categories : BaseTab
    {
        public Categories(CategoriesViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeComponent();
        }
    }
}
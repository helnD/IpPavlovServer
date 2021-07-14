using System.Windows.Controls;
using ViewModels.Partners;

namespace Editor.Desktop.Views.Common
{
    public partial class Partners : BaseTab
    {
        public Partners(PartnersViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeComponent();
        }
    }
}
using System.Windows;
using System.Windows.Controls;
using ViewModels.Common.ViewModel;

namespace Editor.Desktop.Views.Common
{
    public class BaseTab : UserControl
    {
        protected BaseViewModel _viewModel;

        public BaseTab()
        {
            Loaded += BaseTab_OnLoaded;
        }

        private async void BaseTab_OnLoaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }
    }
}
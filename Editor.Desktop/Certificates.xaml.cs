using System.Windows.Controls;
using ViewModels;
using ViewModels.Certificates;

namespace Editor.Desktop
{
    /// <summary>
    /// Tab for certifiactes management.
    /// </summary>
    public partial class Certificates : UserControl
    {

        public Certificates(CertificatesViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        private void DataGrid_OnAddingNewItem(object? sender, AddingNewItemEventArgs e)
        {
            ;
        }
    }
}
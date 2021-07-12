using ViewModels.Certificates;

namespace Editor.Desktop.Views.Common
{
    /// <summary>
    /// Tab for certifiactes management.
    /// </summary>
    public partial class Certificates : BaseTab
    {
        public Certificates(CertificatesViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeComponent();
        }
    }
}
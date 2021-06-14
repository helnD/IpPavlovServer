using System.Windows.Controls;
using ViewModels;

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
    }
}
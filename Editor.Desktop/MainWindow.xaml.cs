using System.Windows;

namespace Editor.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(TabControlsFacade tabControlsFacade)
        {
            InitializeComponent();
            Categories.Content = tabControlsFacade.Categories;
            SalesLeaders.Content = tabControlsFacade.SalesLeaders;
            SalesRepresentatives.Content = tabControlsFacade.Representatives;
            Products.Content = tabControlsFacade.Products;
            Certificates.Content = tabControlsFacade.Certificates;
            Partners.Content = tabControlsFacade.Partners;
        }
    }
}
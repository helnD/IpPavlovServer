using System;
using System.Windows;
using Editor.Desktop.Services;
using Editor.Desktop.Views.Common;
using Infrastructure.Abstractions;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using UseCases.Certificates.GetCertificates;
using ViewModels;
using ViewModels.Categories;
using ViewModels.Certificates;
using ViewModels.Certificates.Models;
using ViewModels.Partners;

namespace Editor.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Database.
            services.AddTransient<IDbContext, AppDbContext>();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(
                    "Host=localhost;Port=5432;Database=IpPavlov;User Id=vladimir;Password=793b3243vova15");
            });

            // Dispatcher
            services.AddTransient<InvokeAsynchronously>(_ =>
                async action => await Dispatcher.InvokeAsync(action)
            );

            // File dialog.
            services.AddTransient<IFileDialog, WinFileDialog>();

            // Image API.
            services.AddTransient<IImageApi, ImageApi>();

            // Controls.
            services.AddSingleton<MainWindow>();
            services.AddTransient<Certificates>();
            services.AddTransient<SalesLeaders>();
            services.AddTransient<SalesRepresentatives>();
            services.AddTransient<Products>();
            services.AddTransient<Categories>();
            services.AddTransient<Partners>();
            services.AddTransient<TabControlsFacade>();

            //ViewModels
            services.AddTransient<CertificatesViewModel>();
            services.AddTransient<CategoriesViewModel>();
            services.AddTransient<PartnersViewModel>();

            // MediatR
            services.AddMediatR(typeof(GetCertificatesQuery));

            // Automapper.
            services.AddAutoMapper(typeof(GetCertificatesQuery), typeof(CertificatesModel));
        }

        private void OnStartup(object sender, StartupEventArgs args)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow?.Show();
        }
    }
}
using System;
using System.Windows;
using Infrastructure.Abstractions;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UseCases.Certificates.GetCertificates;
using ViewModels;

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
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(
                    "Host=localhost;Port=5432;Database=IpPavlov;User Id=vladimir;Password=793b3243vova15");
            });
            services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<AppDbContext>());

            // Dispatcher
            services.AddTransient<InvokeAsynchronously>(_ =>
                async action => await Dispatcher.InvokeAsync(action)
            );

            // Controls.
            services.AddSingleton<MainWindow>();
            services.AddTransient<Certificates>();
            services.AddTransient<SalesLeaders>();
            services.AddTransient<SalesRepresentatives>();
            services.AddTransient<Products>();
            services.AddTransient<Categories>();
            services.AddTransient<TabControlsFacade>();

            //ViewModels
            services.AddTransient<CertificatesViewModel>();

            // MediatR
            services.AddMediatR(typeof(GetCertificatesQuery));
        }

        private void OnStartup(object sender, StartupEventArgs args)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow?.Show();
        }
    }
}
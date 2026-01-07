using App.Application.Interfaces;
using App.Common;
using AppUI.ViewModels;
using AppUI.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows;

namespace AppUI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private IHost? _Host;

    public IServiceProvider ServiceProvider => _Host!.Services;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // 1. Initial Logging setup
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(Constants.Infrastructure.LogFileName, rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            // 2. Build Host
            _Host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
#if DEBUG
                    config.AddJsonFile($"appsettings.{Constants.UI.DebugSuffix}.json", optional: true, reloadOnChange: true);
#elif QA
                    config.AddJsonFile($"appsettings.{Constants.UI.QASuffix}.json", optional: true, reloadOnChange: true);
#endif
                    config.AddEnvironmentVariables();

                })
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(context.Configuration, services);
                })
                .UseSerilog()
                .Build();

            await _Host.StartAsync();

            // 3. Show Splash Screen
            var splash = ServiceProvider.GetRequiredService<SplashWindow>();
            splash.Show();

            // 4. Initialize Services (Simulated work)
            await InitializeApplicationAsync();

            // 5. Show Main Window
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            // 6. Close Splash
            splash.Close();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application start-up failed");
            MessageBox.Show($"Application failed to start: {ex.Message}", "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
    }

    private void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        // UI Services
        services.AddSingleton<SplashWindow>();
        services.AddSingleton<SplashViewModel>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainViewModel>();
        services.AddTransient<AboutWindow>();
        services.AddTransient<AboutViewModel>();

        // Infrastructure Services (Dynamic Loading to honor Clean Architecture boundaries)
        RegisterInfrastructure(configuration, services);
    }

    private void RegisterInfrastructure(IConfiguration configuration, IServiceCollection services)
    {
        try
        {
            // Load App.Infrastructure.dll
            var assemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Infrastructure.InfrastructureDll);
            if (!File.Exists(assemblyPath))
            {
                // Fallback for development if bin folder structure is different
                assemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "App.Infrastructure", "bin", "Debug", "net8.0", Constants.Infrastructure.InfrastructureDll);
            }

            var assembly = Assembly.LoadFrom(assemblyPath);
            var type = assembly.GetType(Constants.Infrastructure.ContainerBuilderType);
            var method = type?.GetMethod(Constants.Infrastructure.RegisterServicesMethod, BindingFlags.Public | BindingFlags.Static);

            if (method != null)
            {
                method.Invoke(null, new object[] { services, configuration });
            }
            else
            {
                throw new InvalidOperationException($"Could not find {Constants.Infrastructure.RegisterServicesMethod} method in {Constants.Infrastructure.InfrastructureDll}.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to load Infrastructure assembly dynamically.");
            throw;
        }
    }

    private async Task InitializeApplicationAsync()
    {
        var localizationService = ServiceProvider.GetRequiredService<ILocalizationService>();
        var logger = ServiceProvider.GetRequiredService<ILogger<App>>();

        logger.LogInformation("Initializing application components...");

        // Load default culture from settings
        var config = ServiceProvider.GetRequiredService<IConfiguration>();
        var defaultLang = config[$"{Constants.Sections.AppSettings}:DefaultLanguage"] ?? Constants.Localization.DefaultCulture;
        localizationService.SetCulture(defaultLang);

        // Simulate database/API check
        await Task.Delay(2000);

        logger.LogInformation("Application initialization complete.");
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_Host != null)
        {
            await _Host.StopAsync();
            _Host.Dispose();
        }

        Log.CloseAndFlush();
        base.OnExit(e);
    }
}

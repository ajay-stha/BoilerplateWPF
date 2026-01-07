using CommunityToolkit.Mvvm.ComponentModel;
using App.Application.Interfaces;
using Microsoft.Extensions.Hosting;

namespace AppUI.ViewModels;

/// <summary>
/// ViewModel for the Splash Screen.
/// </summary>
public partial class SplashViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _Message = "Initializing...";

    [ObservableProperty]
    private string _Version = "1.0.0";

    [ObservableProperty]
    private string _AppTitle = "Enterprise WPF";

    public SplashViewModel(ILocalizationService localizationService, IHostEnvironment environment)
    {
        var baseTitle = localizationService.GetString("AppTitle");
        AppTitle = environment.IsProduction() 
            ? baseTitle 
            : $"{baseTitle} ({environment.EnvironmentName})";
        Message = localizationService.GetString("Loading");
        
        // Version is usually set from assembly metadata in a real app
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version;
        Version = version?.ToString() ?? "1.0.0";
    }
}

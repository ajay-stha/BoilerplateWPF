using CommunityToolkit.Mvvm.ComponentModel;
using App.Application.Interfaces;
using Microsoft.Extensions.Hosting;

namespace AppUI.ViewModels;

/// <summary>
/// ViewModel for the About dialog.
/// </summary>
public partial class AboutViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _Version = "1.0.0";

    [ObservableProperty]
    private string _Description = "WPF Boilerplate.";

    [ObservableProperty]
    private string _AppTitle = string.Empty;

    public AboutViewModel(ILocalizationService localizationService, IHostEnvironment environment, MainViewModel mainViewModel)
    {
        var baseTitle = localizationService.GetString("About");
        Title = environment.IsProduction() 
            ? baseTitle 
            : $"{baseTitle} ({environment.EnvironmentName})";
        
        AppTitle = mainViewModel.Title;
        
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version;
        Version = version?.ToString() ?? "1.0.0";
    }
}

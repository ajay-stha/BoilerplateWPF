using CommunityToolkit.Mvvm.ComponentModel;
using App.Application.Interfaces;

namespace AppUI.ViewModels;

/// <summary>
/// ViewModel for the About dialog.
/// </summary>
public partial class AboutViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _Version = "1.0.0";

    [ObservableProperty]
    private string _Description = "Production-ready WPF Clean Architecture Boilerplate.";

    public AboutViewModel(ILocalizationService localizationService)
    {
        Title = localizationService.GetString("About");
        
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version;
        Version = version?.ToString() ?? "1.0.0";
    }
}

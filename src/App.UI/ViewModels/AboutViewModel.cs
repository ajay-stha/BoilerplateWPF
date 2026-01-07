using CommunityToolkit.Mvvm.ComponentModel;
using App.Application.Interfaces;
using App.Common;
using Microsoft.Extensions.Hosting;

namespace AppUI.ViewModels;

/// <summary>
/// ViewModel for the About dialog.
/// </summary>
public partial class AboutViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _Description = string.Empty;

    public AboutViewModel(ILocalizationService localizationService, IHostEnvironment environment, MainViewModel mainViewModel)
    {
        LoadMetadata();
        Description = localizationService.GetString(Constants.Localization.Keys.AboutDescription);
        var baseTitle = localizationService.GetString(Constants.Localization.Keys.About);
        Title = environment.IsProduction() 
            ? baseTitle 
            : $"{baseTitle} ({environment.EnvironmentName})";
        
        AppTitle = mainViewModel.Title;
    }
}

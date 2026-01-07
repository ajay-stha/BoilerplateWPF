using CommunityToolkit.Mvvm.ComponentModel;
using App.Application.Interfaces;
using App.Common;
using Microsoft.Extensions.Hosting;

namespace AppUI.ViewModels;

/// <summary>
/// ViewModel for the Splash Screen.
/// </summary>
public partial class SplashViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _Message = Constants.UI.DefaultStatus;

    public SplashViewModel(ILocalizationService localizationService, IHostEnvironment environment)
    {
        LoadMetadata();
        var baseTitle = localizationService.GetString(Constants.Localization.Keys.AppTitle);
        AppTitle = environment.IsProduction() 
            ? baseTitle 
            : $"{baseTitle} ({environment.EnvironmentName})";
        Message = localizationService.GetString(Constants.Localization.Keys.Loading);
    }
}

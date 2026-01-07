using App.Application.Interfaces;
using App.Common;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AppUI.ViewModels;

/// <summary>
/// ViewModel for the Splash Screen.
/// </summary>
public partial class SplashViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _Message = Constants.UI.DefaultStatus;

    public SplashViewModel(ILocalizationService localizationService)
    {
        LoadMetadata(localizationService);
        Message = localizationService.GetString(Constants.Localization.Keys.Loading);
    }
}

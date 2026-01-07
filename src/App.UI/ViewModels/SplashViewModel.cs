using CommunityToolkit.Mvvm.ComponentModel;
using App.Application.Interfaces;
using App.Common;

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

using App.Application.Interfaces;
using App.Common;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AppUI.ViewModels;

/// <summary>
/// Base class for all ViewModels.
/// </summary>
public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private string _Title = string.Empty;

    [ObservableProperty]
    private string _AppTitle = string.Empty;

    [ObservableProperty]
    private string _AppVersion = Constants.UI.DefaultVersion;

    [ObservableProperty]
    private string _AppDescription = string.Empty;

    [ObservableProperty]
    private bool _IsBusy;

    protected void LoadMetadata(ILocalizationService localizationService)
    {
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        AppVersion = assembly.GetName().Version?.ToString() ?? Constants.UI.DefaultVersion;
        
        UpdateMetadata(localizationService);
    }

    protected virtual void UpdateMetadata(ILocalizationService localizationService)
    {
        AppDescription = localizationService.GetString(Constants.Localization.Keys.AboutDescription);
        AppTitle = GetBrandedText(localizationService.GetString(Constants.Localization.Keys.AppTitle) ?? Constants.UI.DefaultAppTitle);
    }

    protected string GetBrandedText(string text)
    {
#if DEBUG
        return $"{text} ({Constants.UI.DebugSuffix})";
#elif QA
        return $"{text} ({Constants.UI.QASuffix})";
#else
        return text;
#endif
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using App.Common;

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
    private bool _IsBusy;

    protected void LoadMetadata()
    {
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version;
        AppVersion = version?.ToString() ?? Constants.UI.DefaultVersion;
        
        // Default AppTitle if not overwritten by localized version
        if (string.IsNullOrEmpty(AppTitle))
        {
            AppTitle = assembly.GetCustomAttributes(typeof(System.Reflection.AssemblyTitleAttribute), false)
                .FirstOrDefault() is System.Reflection.AssemblyTitleAttribute titleAttr 
                ? titleAttr.Title 
                : Constants.UI.DefaultAppTitle;
        }
    }
}

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
    private bool _IsBusy;
}

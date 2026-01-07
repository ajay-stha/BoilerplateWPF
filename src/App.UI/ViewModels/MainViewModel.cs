using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using App.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using AppUI.Views;

namespace AppUI.ViewModels;

/// <summary>
/// ViewModel for the Main Window.
/// </summary>
public partial class MainViewModel : BaseViewModel
{
    private readonly ILogger<MainViewModel> _Logger;
    private readonly ISampleService _SampleService;
    private readonly IUnitOfWork _UnitOfWork;
    private readonly ILocalizationService _LocalizationService;
    private readonly IServiceProvider _ServiceProvider;

    [ObservableProperty]
    private string _WelcomeMessage = string.Empty;

    public MainViewModel(
        ILogger<MainViewModel> logger,
        ISampleService sampleService,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IServiceProvider serviceProvider)
    {
        _Logger = logger;
        _SampleService = sampleService;
        _UnitOfWork = unitOfWork;
        _LocalizationService = localizationService;
        _ServiceProvider = serviceProvider;

        Title = _LocalizationService.GetString("AppTitle");
        WelcomeMessage = _LocalizationService.GetString("WelcomeMessage");
        
        _LocalizationService.CultureChanged += (s, e) => {
            Title = _LocalizationService.GetString("AppTitle");
            WelcomeMessage = _LocalizationService.GetString("WelcomeMessage");
        };
    }

    [RelayCommand]
    private void SwitchToGerman() => _LocalizationService.SetCulture("de-DE");

    [RelayCommand]
    private void SwitchToEnglish() => _LocalizationService.SetCulture("en-US");

    [RelayCommand]
    private void ShowAbout()
    {
        var aboutWindow = _ServiceProvider.GetRequiredService<AboutWindow>();
        aboutWindow.Owner = System.Windows.Application.Current.MainWindow;
        aboutWindow.ShowDialog();
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        IsBusy = true;
        try
        {
            var samples = await _SampleService.GetSamplesAsync();
            _Logger.LogInformation("Loaded {Count} samples.", samples.Count());
        }
        finally
        {
            IsBusy = false;
        }
    }
}

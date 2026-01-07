using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using App.Application.Interfaces;
using App.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using AppUI.Views;
using Microsoft.Extensions.Hosting;

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
    private readonly IHostEnvironment _Environment;

    [ObservableProperty]
    private string _WelcomeMessage = string.Empty;

    public MainViewModel(
        ILogger<MainViewModel> logger,
        ISampleService sampleService,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IServiceProvider serviceProvider,
        IHostEnvironment environment)
    {
        _Logger = logger;
        _SampleService = sampleService;
        _UnitOfWork = unitOfWork;
        _LocalizationService = localizationService;
        _ServiceProvider = serviceProvider;
        _Environment = environment;

        UpdateTitle();
        WelcomeMessage = _LocalizationService.GetString(Constants.Localization.Keys.WelcomeMessage);
        
        _LocalizationService.CultureChanged += (s, e) => {
            UpdateTitle();
            WelcomeMessage = _LocalizationService.GetString(Constants.Localization.Keys.WelcomeMessage);
        };
    }

    private void UpdateTitle()
    {
        var baseTitle = _LocalizationService.GetString(Constants.Localization.Keys.AppTitle);
        Title = _Environment.IsProduction() 
            ? baseTitle 
            : $"{baseTitle} ({_Environment.EnvironmentName})";
    }

    [RelayCommand]
    private void SwitchToGerman() => _LocalizationService.SetCulture(Constants.Localization.German);

    [RelayCommand]
    private void SwitchToEnglish() => _LocalizationService.SetCulture(Constants.Localization.English);

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

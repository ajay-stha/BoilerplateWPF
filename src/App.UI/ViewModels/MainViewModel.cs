using App.Application.Interfaces;
using App.Common;
using App.Domain.Entities;
using App.UI.Factory;
using AppUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppUI.ViewModels;

/// <summary>
/// ViewModel for the Main Window.
/// </summary>
public partial class MainViewModel : BaseViewModel
{
    private readonly ILogger<MainViewModel> _Logger;
    private readonly IUnitOfWork _UnitOfWork;
    private readonly ILocalizationService _LocalizationService;
    private readonly IWindowFactory _WindowFactory;

    [ObservableProperty]
    private string _WelcomeMessage = string.Empty;

    [ObservableProperty]
    private IEnumerable<UserSetting> _UserSettings = System.Linq.Enumerable.Empty<UserSetting>();

    public MainViewModel(
        ILogger<MainViewModel> logger,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IWindowFactory windowFactory)
    {
        _Logger = logger;
        _UnitOfWork = unitOfWork;
        _LocalizationService = localizationService;
        _WindowFactory = windowFactory;

        UpdateMetadata(_LocalizationService);
        WelcomeMessage = _LocalizationService.GetString(Constants.Localization.Keys.WelcomeMessage);

        _LocalizationService.CultureChanged += (s, e) =>
        {
            UpdateMetadata(_LocalizationService);
            WelcomeMessage = _LocalizationService.GetString(Constants.Localization.Keys.WelcomeMessage);
        };
    }

    protected override void UpdateMetadata(ILocalizationService localizationService)
    {
        base.UpdateMetadata(localizationService);
        Title = AppTitle;
    }

    [RelayCommand]
    private void SwitchToGerman() => _LocalizationService.SetCulture(Constants.Localization.German);

    [RelayCommand]
    private void SwitchToEnglish() => _LocalizationService.SetCulture(Constants.Localization.English);

    [RelayCommand]
    private void ShowAbout()
    {
        _WindowFactory.ShowAboutWindow();
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        IsBusy = true;
        try
        {
            var repo = _UnitOfWork?.Repository<UserSetting>();
            if (repo != null)
            {
                try
                {
                    var userSettings = await repo.GetAllAsync();
                    UserSettings = userSettings;
                    _Logger.LogInformation("Loaded {Count} user settings.", UserSettings?.Count() ?? 0);
                }
                catch (PlatformNotSupportedException ex)
                {
                    _Logger.LogWarning(ex, "Skipping user settings load: platform does not support the SQL client.");
                }
                catch (Exception ex)
                {
                    _Logger.LogError(ex, "Failed loading user settings.");
                }
            }
            else
            {
                _Logger.LogDebug("No UserSetting repository available; skipping user settings load.");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}

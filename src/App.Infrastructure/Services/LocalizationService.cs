using System.Globalization;
using System.Resources;
using App.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace App.Infrastructure.Services;

/// <summary>
/// Implementation of localization service using .resx resource files.
/// </summary>
public class LocalizationService : ILocalizationService
{
    private readonly ILogger<LocalizationService> _Logger;
    private readonly ResourceManager _ResourceManager;
    private CultureInfo _CurrentCulture;

    public LocalizationService(ILogger<LocalizationService> logger)
    {
        _Logger = logger;
        _ResourceManager = new ResourceManager("App.Infrastructure.Resources.Strings", typeof(LocalizationService).Assembly);
        _CurrentCulture = CultureInfo.CurrentUICulture;
    }

    public CultureInfo CurrentCulture => _CurrentCulture;

    public event EventHandler? CultureChanged;

    public void SetCulture(string cultureName)
    {
        try
        {
            var culture = new CultureInfo(cultureName);
            _CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
            
            _Logger.LogInformation("Culture switched to {Culture}", cultureName);
            CultureChanged?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            _Logger.LogError(ex, "Failed to set culture to {Culture}", cultureName);
        }
    }

    public string GetString(string key)
    {
        return _ResourceManager.GetString(key, _CurrentCulture) ?? $"!{key}!";
    }
}

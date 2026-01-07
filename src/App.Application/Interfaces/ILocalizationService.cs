using System.Globalization;

namespace App.Application.Interfaces;

/// <summary>
/// Interface for localization service to manage application culture at runtime.
/// </summary>
public interface ILocalizationService
{
    /// <summary>
    /// Gets the current culture of the application.
    /// </summary>
    CultureInfo CurrentCulture { get; }

    /// <summary>
    /// Switches the application culture at runtime.
    /// </summary>
    /// <param name="cultureName">Name of the culture (e.g., "en-US").</param>
    void SetCulture(string cultureName);

    /// <summary>
    /// Gets a localized string by its key.
    /// </summary>
    /// <param name="key">The resource key.</param>
    /// <returns>The localized string.</returns>
    string GetString(string key);

    /// <summary>
    /// Event raised when the culture is changed.
    /// </summary>
    event EventHandler? CultureChanged;
}

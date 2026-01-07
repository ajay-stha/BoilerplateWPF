namespace App.Common;

/// <summary>
/// Centralized constants for the application to avoid magic strings.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Configuration section names.
    /// </summary>
    public static class Sections
    {
        public const string ConnectionStrings = "ConnectionStrings";
        public const string Logging = "Logging";
        public const string AppSettings = "AppSettings";
    }

    /// <summary>
    /// Logging categories or specific log messages.
    /// </summary>
    public static class Logging
    {
        public const string AppStartup = "Application starting...";
        public const string AppShutdown = "Application shutting down...";
    }

    /// <summary>
    /// Localization keys and default values.
    /// </summary>
    public static class Localization
    {
        public const string DefaultCulture = "en-US";
        public const string English = "en-US";
        public const string French = "fr-FR";
        public const string German = "de-DE";
        public const string Spanish = "es-ES";
    }
}

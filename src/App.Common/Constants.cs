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

        /// <summary>
        /// Resource keys for localization.
        /// </summary>
        public static class Keys
        {
            public const string AppTitle = "AppTitle";
            public const string WelcomeMessage = "WelcomeMessage";
            public const string Loading = "Loading";
            public const string InitializationComplete = "InitializationComplete";
            public const string Version = "Version";
            public const string About = "About";
            public const string AboutDescription = "AboutDescription";
            public const string Settings = "Settings";
            public const string Language = "Language";
        }
    }

    /// <summary>
    /// UI-related constants (View names, default messages).
    /// </summary>
    public static class UI
    {
        public const string DefaultVersion = "1.0.0";
        public const string DefaultStatus = "Initializing...";
        public const string DefaultAppTitle = "Enterprise WPF";
        public const string SplashWindowTitle = "SplashWindow";
    }

    /// <summary>
    /// Infrastructure-related constants (DLL names, types).
    /// </summary>
    public static class Infrastructure
    {
        public const string InfrastructureDll = "App.Infrastructure.dll";
        public const string ContainerBuilderType = "App.Infrastructure.DI.ContainerBuilder";
        public const string RegisterServicesMethod = "RegisterServices";
        public const string LogFileName = "logs/log-.txt";
    }
}

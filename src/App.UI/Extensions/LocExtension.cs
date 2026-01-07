using System.Windows.Data;
using System.Windows.Markup;
using App.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AppUI.Extensions;

/// <summary>
/// Markup extension for XAML localization.
/// </summary>
public class LocExtension : MarkupExtension
{
    public string Key { get; set; } = string.Empty;

    public LocExtension()
    {
    }

    public LocExtension(string key)
    {
        Key = key;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (string.IsNullOrEmpty(Key)) return string.Empty;

        // In Design Mode, we can't easily resolve services, return key
        if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
        {
            return $"[{Key}]";
        }

        var binding = new Binding("Value")
        {
            Source = new LocProxy(Key),
            Mode = BindingMode.OneWay
        };

        return binding.ProvideValue(serviceProvider);
    }
}

/// <summary>
/// Proxy class to support dynamic updates when culture changes.
/// </summary>
public class LocProxy : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    private readonly string _Key;
    private readonly ILocalizationService _LocalizationService;

    public LocProxy(string key)
    {
        _Key = key;
        // Resolve from App's service provider
        _LocalizationService = ((App)System.Windows.Application.Current).ServiceProvider.GetRequiredService<ILocalizationService>();
        _LocalizationService.CultureChanged += (s, e) => OnPropertyChanged(nameof(Value));
    }

    public string Value => _LocalizationService.GetString(_Key);
}

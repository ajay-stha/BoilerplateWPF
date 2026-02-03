using AppUI.Views;
using Microsoft.Extensions.DependencyInjection;

namespace App.UI.Factory;

public class WindowFactory : IWindowFactory
{
    private readonly IServiceProvider _ServiceProvider;

    public WindowFactory(IServiceProvider serviceProvider)
    {
        _ServiceProvider = serviceProvider;
    }

    public void ShowAboutWindow()
    {
        var window = _ServiceProvider.GetRequiredService<AboutWindow>();
        window.ShowDialog();
    }
}


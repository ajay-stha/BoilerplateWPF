using System.Windows;
using AppUI.ViewModels;

namespace AppUI.Views;

/// <summary>
/// Interaction logic for SplashWindow.xaml
/// </summary>
public partial class SplashWindow : Window
{
    public SplashWindow(SplashViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}

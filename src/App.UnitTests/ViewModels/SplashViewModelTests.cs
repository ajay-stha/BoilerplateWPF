using App.Application.Interfaces;
using App.Common;
using AppUI.ViewModels;
using Moq;
using Xunit;

namespace App.UnitTests.ViewModels;

public class SplashViewModelTests
{
    private readonly Mock<ILocalizationService> _MockLocalizationService;

    public SplashViewModelTests()
    {
        _MockLocalizationService = new Mock<ILocalizationService>();
        _MockLocalizationService.Setup(s => s.GetString(It.IsAny<string>())).Returns((string key) => $"Localized_{key}");
    }

    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Act
        var vm = new SplashViewModel(_MockLocalizationService.Object);

        // Assert
        var expectedTitle = Helpers.GetBrandedTitle();
        Assert.Equal(expectedTitle, vm.AppTitle);
        Assert.Equal($"Localized_{Constants.Localization.Keys.Loading}", vm.Message);
        Assert.False(string.IsNullOrEmpty(vm.AppVersion));
    }
}

using Xunit;
using App.Application.Interfaces;
using AppUI.ViewModels;
using Moq;

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
        Assert.Equal("Localized_AppTitle", vm.AppTitle);
        Assert.Equal("Localized_Loading", vm.Message);
        Assert.False(string.IsNullOrEmpty(vm.Version));
    }
}

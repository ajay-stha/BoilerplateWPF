using App.Application.Interfaces;
using App.Common;
using AppUI.ViewModels;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace App.UnitTests.ViewModels;

public class SplashViewModelTests
{
    private readonly Mock<ILocalizationService> _MockLocalizationService;
    private readonly Mock<IHostEnvironment> _MockEnvironment;

    public SplashViewModelTests()
    {
        _MockLocalizationService = new Mock<ILocalizationService>();
        _MockEnvironment = new Mock<IHostEnvironment>();
        _MockEnvironment.Setup(e => e.EnvironmentName).Returns("Production");
        _MockLocalizationService.Setup(s => s.GetString(It.IsAny<string>())).Returns((string key) => $"Localized_{key}");
    }

    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Act
        var vm = new SplashViewModel(_MockLocalizationService.Object);

        // Assert
        var expectedTitle = $"Localized_{Constants.Localization.Keys.AppTitle}";
#if DEBUG
        expectedTitle = $"{expectedTitle} {Constants.UI.DebugSuffix}";
#elif QA
        expectedTitle = $"{expectedTitle} {Constants.UI.QASuffix}";
#endif
        Assert.Equal(expectedTitle, vm.AppTitle);
        Assert.Equal($"Localized_{Constants.Localization.Keys.Loading}", vm.Message);
        Assert.False(string.IsNullOrEmpty(vm.AppVersion));
    }
}

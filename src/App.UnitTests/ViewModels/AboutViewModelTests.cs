using App.Application.Interfaces;
using App.Common;
using AppUI.ViewModels;
using Moq;
using Xunit;

namespace App.UnitTests.ViewModels;

public class AboutViewModelTests
{
    private readonly Mock<ILocalizationService> _LocalizationServiceMock;

    public AboutViewModelTests()
    {
        _LocalizationServiceMock = new Mock<ILocalizationService>();

        _LocalizationServiceMock
            .Setup(l => l.GetString(Constants.Localization.Keys.About))
            .Returns("About");
    }

    [Fact]
    public void Constructor_Sets_Title_From_LocalizationService()
    {
        // Act
        var vm = new AboutViewModel(_LocalizationServiceMock.Object);

        // Assert
        Assert.NotNull(vm.Title);
        Assert.Contains("About", vm.Title);
    }

    [Fact]
    public void Constructor_Calls_LocalizationService()
    {
        // Act
        _ = new AboutViewModel(_LocalizationServiceMock.Object);

        // Assert
        _LocalizationServiceMock.Verify(
            l => l.GetString(Constants.Localization.Keys.About),
            Times.Once);
    }
}

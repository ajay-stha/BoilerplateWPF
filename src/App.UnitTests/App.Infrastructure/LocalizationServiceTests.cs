using App.Infrastructure.Services;
using App.UnitTests.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Globalization;
using Xunit;

namespace App.UnitTests.App.Infrastructure;

public class LocalizationServiceTests
{
    private readonly Mock<ILogger<LocalizationService>> _LoggerMock;

    public LocalizationServiceTests()
    {
        _LoggerMock = new Mock<ILogger<LocalizationService>>();
    }

    [Fact]
    public void SetCulture_ValidCulture_UpdatesCurrentCultureAndRaisesEvent()
    {
        // Arrange
        var service = new LocalizationService(_LoggerMock.Object);
        string newCulture = "fr-FR";
        bool eventRaised = false;
        service.CultureChanged += (s, e) => eventRaised = true;

        // Act
        service.SetCulture(newCulture);

        // Assert
        Assert.Equal(newCulture, service.CurrentCulture.Name);
        Assert.Equal(newCulture, CultureInfo.CurrentCulture.Name);
        Assert.True(eventRaised);

        _LoggerMock.VerifyLogMessageContains(
            LogLevel.Information,
            newCulture,
            Times.Once);
    }

    [Fact]
    public void SetCulture_InvalidCulture_LogsErrorAndDoesNotChangeCurrentCulture()
    {
        // Arrange
        var service = new LocalizationService(_LoggerMock.Object);
        var originalCulture = service.CurrentCulture;
        string invalidCulture = "invalid-CULTURE";

        // Act
        service.SetCulture(invalidCulture);

        // Assert
        Assert.Equal(originalCulture, service.CurrentCulture);

        _LoggerMock.VerifyLogLevel(LogLevel.Error, Times.Once);
    }

    [Fact]
    public void GetString_ExistingKey_ReturnsValue()
    {
        // Arrange
        var service = new LocalizationService(_LoggerMock.Object);

        string key = "Version";

        // Act
        var result = service.GetString(key);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual($"!{key}!", result);
    }

    [Fact]
    public void GetString_MissingKey_ReturnsFallback()
    {
        // Arrange
        var service = new LocalizationService(_LoggerMock.Object);
        string missingKey = "ThisKeyDoesNotExist";

        // Act
        var result = service.GetString(missingKey);

        // Assert
        Assert.Equal($"!{missingKey}!", result);
    }
}

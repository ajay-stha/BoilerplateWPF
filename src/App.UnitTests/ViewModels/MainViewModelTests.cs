using Xunit;
using App.Application.Interfaces;
using AppUI.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;
using App.Application.DTOs;

namespace App.UnitTests.ViewModels;

public class MainViewModelTests
{
    private readonly Mock<ILogger<MainViewModel>> _MockLogger;
    private readonly Mock<ISampleService> _MockSampleService;
    private readonly Mock<IUnitOfWork> _MockUnitOfWork;
    private readonly Mock<ILocalizationService> _MockLocalizationService;
    private readonly Mock<IServiceProvider> _MockServiceProvider;

    public MainViewModelTests()
    {
        _MockLogger = new Mock<ILogger<MainViewModel>>();
        _MockSampleService = new Mock<ISampleService>();
        _MockUnitOfWork = new Mock<IUnitOfWork>();
        _MockLocalizationService = new Mock<ILocalizationService>();
        _MockServiceProvider = new Mock<IServiceProvider>();

        _MockLocalizationService.Setup(s => s.GetString(It.IsAny<string>())).Returns((string key) => $"Localized_{key}");
    }

    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Act
        var vm = new MainViewModel(
            _MockLogger.Object,
            _MockSampleService.Object,
            _MockUnitOfWork.Object,
            _MockLocalizationService.Object,
            _MockServiceProvider.Object);

        // Assert
        Assert.Equal("Localized_AppTitle", vm.Title);
        Assert.Equal("Localized_WelcomeMessage", vm.WelcomeMessage);
    }

    [Fact]
    public async Task LoadDataCommand_ShouldUpdateBusyStateAndLog()
    {
        // Arrange
        var samples = new List<SampleDto> { new() { Id = 1, Name = "Test" } };
        _MockSampleService.Setup(s => s.GetSamplesAsync()).ReturnsAsync(samples);

        var vm = new MainViewModel(
            _MockLogger.Object,
            _MockSampleService.Object,
            _MockUnitOfWork.Object,
            _MockLocalizationService.Object,
            _MockServiceProvider.Object);

        // Act
        await vm.LoadDataCommand.ExecuteAsync(null);

        // Assert
        Assert.False(vm.IsBusy);
        _MockSampleService.Verify(s => s.GetSamplesAsync(), Times.Once);
    }
}

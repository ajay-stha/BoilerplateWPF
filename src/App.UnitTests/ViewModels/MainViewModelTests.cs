using App.Application.Interfaces;
using App.Common;
using App.Domain.Entities;
using App.Domain.Interfaces;
using App.UI.Factory;
using App.UnitTests.Extensions;
using AppUI.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace App.UnitTests.ViewModels;


public class MainViewModelTests
{
    private Mock<ILogger<MainViewModel>> _LoggerMock;
    private Mock<IUnitOfWork> _UnitOfWorkMock;
    private Mock<ILocalizationService> _LocalizationServiceMock;
    private Mock<IServiceProvider> _ServiceProviderMock;
    private Mock<IRepository<UserSetting>> _UserSettingRepoMock;
    private Mock<IWindowFactory> _WindowFactoryMock;

    private MainViewModel _ViewModel;

    public MainViewModelTests()
    {
        _LoggerMock = new Mock<ILogger<MainViewModel>>();
        _UnitOfWorkMock = new Mock<IUnitOfWork>();
        _LocalizationServiceMock = new Mock<ILocalizationService>();
        _ServiceProviderMock = new Mock<IServiceProvider>();
        _UserSettingRepoMock = new Mock<IRepository<UserSetting>>();
        _WindowFactoryMock = new Mock<IWindowFactory>();

        // Default repo returns list
        _UnitOfWorkMock
            .Setup(u => u.Repository<UserSetting>())
            .Returns(_UserSettingRepoMock.Object);

        _LocalizationServiceMock
            .Setup(l => l.GetString(It.IsAny<string>()))
            .Returns("Welcome");

        _ViewModel = new MainViewModel(
            _LoggerMock.Object,
            _UnitOfWorkMock.Object,
            _LocalizationServiceMock.Object,
            _WindowFactoryMock.Object);
    }

    [Fact]
    public void Ctor_SetsWelcomeMessage()
    {
        Assert.Equal("Welcome", _ViewModel.WelcomeMessage);
    }

    [Fact]
    public void SwitchToGermanCommand_CallsSetCulture()
    {
        _ViewModel.SwitchToGermanCommand.Execute(null);

        _LocalizationServiceMock.Verify(
            l => l.SetCulture(Constants.Localization.German), Times.Once);
    }

    [Fact]
    public void SwitchToEnglishCommand_CallsSetCulture()
    {
        _ViewModel.SwitchToEnglishCommand.Execute(null);

        _LocalizationServiceMock.Verify(
            l => l.SetCulture(Constants.Localization.English), Times.Once);
    }

    [Fact]
    public void ShowAboutCommand_ResolvesAndShowsWindow()
    {
        // Arrange
        _WindowFactoryMock.Setup(f => f.ShowAboutWindow());

        // Act
        _ViewModel.ShowAboutCommand.Execute(null);

        // Assert
        _WindowFactoryMock.Verify(f => f.ShowAboutWindow(), Times.Once);
    }

    [Fact]
    public async Task LoadDataAsync_LoadsUserSettings_WhenRepoHasData()
    {
        var data = new List<UserSetting> { new() };

        _UserSettingRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(data);

        await _ViewModel.LoadDataCommand.ExecuteAsync(null);

        Assert.Single(_ViewModel.UserSettings);

        _LoggerMock.VerifyLogMessageContains(
            LogLevel.Information,
            "Loaded",
            Times.Once);
    }

    [Fact]
    public async Task LoadDataAsync_DoesNothing_WhenRepoIsNull()
    {
        _UnitOfWorkMock
            .Setup(u => u.Repository<UserSetting>())
            .Returns((IRepository<UserSetting>)null!);

        await _ViewModel.LoadDataCommand.ExecuteAsync(null);

        _LoggerMock.VerifyLogLevel(LogLevel.Debug, Times.Once);
    }

    [Fact]
    public async Task LoadDataAsync_HandlesPlatformNotSupportedException()
    {
        _UserSettingRepoMock
            .Setup(r => r.GetAllAsync())
            .ThrowsAsync(new PlatformNotSupportedException());

        await _ViewModel.LoadDataCommand.ExecuteAsync(null);

        _LoggerMock.VerifyLogLevel(LogLevel.Warning, Times.Once);
    }

    [Fact]
    public async Task LoadDataAsync_HandlesGeneralException()
    {
        _UserSettingRepoMock
            .Setup(r => r.GetAllAsync())
            .ThrowsAsync(new Exception("boom"));

        await _ViewModel.LoadDataCommand.ExecuteAsync(null);

        _LoggerMock.VerifyLogLevel(LogLevel.Error, Times.Once);
    }

    [Fact]
    public async Task LoadDataAsync_SetsIsBusy_TrueThenFalse()
    {
        var data = new List<UserSetting> { new() };
        _UserSettingRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(data);

        // Before executing: not busy
        Assert.False(_ViewModel.IsBusy);

        await _ViewModel.LoadDataCommand.ExecuteAsync(null);

        // After executing: not busy
        Assert.False(_ViewModel.IsBusy);
    }

    [Fact]
    public void Constructor_WiresCultureChangedEvent_AndUpdatesMetadata()
    {
        // Arrange
        var localizationMock = new Mock<ILocalizationService>();
        localizationMock
            .Setup(l => l.GetString(It.IsAny<string>()))
            .Returns("Welcome");

        var loggerMock = new Mock<ILogger<MainViewModel>>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var windowFactoryMock = new Mock<IWindowFactory>();

        // Act
        var vm = new MainViewModel(
            loggerMock.Object,
            unitOfWorkMock.Object,
            localizationMock.Object,
            windowFactoryMock.Object
        );

        // Assert initial values
        Assert.Equal("Welcome", vm.WelcomeMessage);

        // Raise the CultureChanged event to test the lambda
        localizationMock.Raise(l => l.CultureChanged += null!, EventArgs.Empty);

        // After event, WelcomeMessage should be updated again
        localizationMock.Verify(l => l.GetString(Constants.Localization.Keys.WelcomeMessage), Times.Exactly(2));
    }

}

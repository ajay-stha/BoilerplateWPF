using App.Application.Interfaces;
using App.Infrastructure.DI;
using App.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace App.UnitTests.App.Infrastructure;

public class ContainerBuilderUnitTest
{
    [Fact]
    public void RegisterServices_RegistersAllDependencies_CanResolveServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Create a mock IConfiguration (if needed in your app)
        var mockConfiguration = new Mock<IConfiguration>();
        IConfiguration configuration = mockConfiguration.Object;

        // Act
        ContainerBuilder.RegisterServices(services, configuration);
        var provider = services.BuildServiceProvider();

        // Assert
        // DbContext should be resolvable
        var dbContext = provider.GetService<AppDbContext>();
        Assert.NotNull(dbContext);

        // UnitOfWork should be resolvable and uses DbContext
        var unitOfWork = provider.GetService<IUnitOfWork>();
        Assert.NotNull(unitOfWork);

        // LocalizationService singleton
        var localizationService = provider.GetService<ILocalizationService>();
        Assert.NotNull(localizationService);

        // Logging should be resolvable
        var logger = provider.GetService<ILogger<ContainerBuilder>>();
        Assert.NotNull(logger);
    }
}

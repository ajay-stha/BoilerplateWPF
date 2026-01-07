using App.Application.Interfaces;
using App.Infrastructure.Persistence;
using App.Infrastructure.Repositories.Implementation;
using App.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace App.Infrastructure.DI;

/// <summary>
/// Static class to build the service container, similar to the reference pattern.
/// </summary>
public static class ContainerBuilder
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(ServiceLifetime.Transient);

        // Unit of Work & Repositories
        services.AddTransient<IUnitOfWork, UnitOfWork>(sp =>
            new UnitOfWork(sp.GetRequiredService<AppDbContext>()));

        // Localization
        services.AddSingleton<ILocalizationService, LocalizationService>();

        // Logging
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });
    }

}

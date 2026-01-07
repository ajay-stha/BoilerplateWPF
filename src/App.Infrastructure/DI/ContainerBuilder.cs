using App.Application.Interfaces;
using App.Infrastructure.Persistence;
using App.Infrastructure.Repositories.Implementation;
using App.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Serilog;

namespace App.Infrastructure.DI;

/// <summary>
/// Static class to build the service container, similar to the reference pattern.
/// </summary>
public static class ContainerBuilder
{
    public static IServiceProvider Build(IConfiguration configuration)
    {
        var services = new ServiceCollection();
        RegisterServices(services, configuration);
        return services.BuildServiceProvider();
    }

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

        // API Clients
        services.AddHttpClient<ISampleService, SampleService>()
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}

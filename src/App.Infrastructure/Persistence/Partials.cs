using App.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Persistence;

public partial class AppDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
#if DEBUG
                    .AddJsonFile($"appsettings.{Constants.UI.DebugSuffix}.json", optional: true, reloadOnChange: true)
#elif QA
                    .AddJsonFile($"appsettings.{Constants.UI.QASuffix}.json", optional: true, reloadOnChange: true)
#endif
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

            if (!string.IsNullOrEmpty(connectionString))
            {
                optionsBuilder.UseSqlServer(connectionString, (sqlServerOptions) =>
                {
                    sqlServerOptions.UseNetTopologySuite();
                    sqlServerOptions.EnableRetryOnFailure();
                    sqlServerOptions.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
                });
            }
        }

        optionsBuilder.EnableSensitiveDataLogging();
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

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
                .Build();

            string connectionString = string.Empty;
#if DEBUG
            connectionString = configuration.GetConnectionString("DevConnection") 
                               ?? configuration.GetConnectionString("DefaultConnection") 
                               ?? string.Empty;
#elif QA
            connectionString = configuration.GetConnectionString("QAConnection") 
                               ?? configuration.GetConnectionString("DefaultConnection") 
                               ?? string.Empty;
#else
            connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
#endif
            
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

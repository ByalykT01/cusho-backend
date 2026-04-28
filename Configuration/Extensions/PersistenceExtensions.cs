using cusho.Configuration.Options;
using cusho.Data;
using Microsoft.EntityFrameworkCore;

namespace cusho.Configuration.Extensions;

public static class PersistenceExtensions
{
    extension(IHostApplicationBuilder builder)
    {
        public IHostApplicationBuilder AddPersistence()
        {
            var database = builder.Configuration.GetSection(DatabaseOptions.SectionName).Get<DatabaseOptions>() ??
                           throw new InvalidOperationException("Database Configuration not found");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(database.ToConnectionString));

            return builder;
        }
    }

    extension(WebApplication app)
    {
        public async Task MigrateDatabaseAsync()
        {
            using var scope = app.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await db.Database.MigrateAsync();
        }
    }
}
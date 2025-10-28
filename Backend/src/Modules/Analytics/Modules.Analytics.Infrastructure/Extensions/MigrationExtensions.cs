using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modules.Analytics.Infrastructure.Data;

namespace Modules.Analytics.Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static IApplicationBuilder ApplyAnalyticsModuleMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<AnalyticsDbContext>();

        dbContext.Database.Migrate();

        return app;
    }
}
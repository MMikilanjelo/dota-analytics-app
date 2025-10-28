using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modules.ExternalAccounts.Infrastructure.Data;

namespace Modules.ExternalAccounts.Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static IApplicationBuilder ApplyExternalAccountsModuleMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<ExternalAccountsDbContext>();

        dbContext.Database.Migrate();

        return app;
    }
}
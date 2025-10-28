using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modules.Users.Infrastructure.Data;

namespace Modules.Users.Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static IApplicationBuilder ApplyUsersModuleMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();

        dbContext.Database.Migrate();

        return app;
    }
}
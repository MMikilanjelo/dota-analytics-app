using Common.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Modules.Analytics.Infrastructure.Data;

public class AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(Schemas.Analytics);

        base.OnModelCreating(builder);
    }
}
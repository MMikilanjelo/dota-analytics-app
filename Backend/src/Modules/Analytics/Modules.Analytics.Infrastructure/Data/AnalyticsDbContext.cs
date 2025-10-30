using Infrastructure.Data.Configurations;
using SharedKernel.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Modules.Analytics.Infrastructure.Data;

public class AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(Schemas.Analytics);

        builder.ApplyConfiguration(new InboxMessageConfiguration());
        
        builder.ApplyConfiguration(new OutboxMessageConfiguration());

        base.OnModelCreating(builder);
    }
}
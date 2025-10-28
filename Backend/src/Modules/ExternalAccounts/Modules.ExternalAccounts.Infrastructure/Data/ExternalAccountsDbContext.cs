using Microsoft.EntityFrameworkCore;
using Modules.ExternalAccounts.Domain.Aggregates;
using SharedKernel;
using SharedKernel.Contracts;
using SharedKernel.Contracts.Messaging;
using StrictId.EFCore;

namespace Modules.ExternalAccounts.Infrastructure.Data;

public class ExternalAccountsDbContext(DbContextOptions<ExternalAccountsDbContext> options) : DbContext(options)
{
    public DbSet<ExternalAccountLinkEntity> Accounts => Set<ExternalAccountLinkEntity>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.ExternalAccounts);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExternalAccountsDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.ConfigureStrictId();
    }
}
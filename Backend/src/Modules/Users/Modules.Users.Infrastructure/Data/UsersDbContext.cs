using Microsoft.EntityFrameworkCore;
using Modules.Users.Domain.Aggregates.RefreshTokenAggregate;
using Modules.Users.Domain.Aggregates.UserAggregate;
using SharedKernel;
using SharedKernel.Contracts;
using SharedKernel.Contracts.Messaging;
using StrictId.EFCore;

namespace Modules.Users.Infrastructure.Data;

public class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<RefreshTokenEntity> RefreshTokens => Set<RefreshTokenEntity>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Users);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.ConfigureStrictId();
    }
}
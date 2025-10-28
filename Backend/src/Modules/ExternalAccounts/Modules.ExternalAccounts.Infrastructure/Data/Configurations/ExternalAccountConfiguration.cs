using Common.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.ExternalAccounts.Domain.Aggregates;
using Modules.ExternalAccounts.Domain.ValueObjects;
using StrictId;
using StrictId.EFCore;

namespace Modules.ExternalAccounts.Infrastructure.Data.Configurations;

public class ExternalAccountConfiguration : IEntityTypeConfiguration<ExternalAccountLinkEntity>
{
    public void Configure(EntityTypeBuilder<ExternalAccountLinkEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasStrictIdValueGenerator();

        builder
            .Property(e => e.OwnerId)
            .HasConversion(
                v => v.Value.ToString(),
                v => new Id(v)
            )
            .ValueGeneratedNever();

        builder
            .Property(x => x.SyncInterval)
            .HasConversion(
                v => v.Ticks,
                v => TimeSpan.FromTicks(v)
            )
            .IsRequired();

        builder.ComplexProperty(e => e.ExternalAccountId, owned =>
        {
            owned.Property(p => p.Kind)
                .HasColumnName("ExternalAccountKind")
                .HasConversion(
                    v => v.Name,
                    raw => Enumeration.FromDisplayName<ProviderKind>(raw)
                )
                .HasMaxLength(32)
                .IsRequired();

            owned.Property(p => p.Value)
                .HasColumnName("ExternalAccountValue")
                .HasMaxLength(128)
                .IsRequired();
        });
    }
}
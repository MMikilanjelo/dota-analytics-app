using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain.Aggregates.RefreshTokenAggregate;
using StrictId.EFCore;

namespace Modules.Users.Infrastructure.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasStrictIdValueGenerator();

        builder.Property(e => e.UserId)
            .ValueGeneratedNever()
            .HasStrictIdValueGenerator();

        builder.Property(rt => rt.Token).HasMaxLength(200);

        builder
            .HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(rt => rt.Token).IsUnique();
        builder.HasIndex(rt => rt.UserId);
        
        builder.ToTable("RefreshTokens");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain.Aggregates.UserAggregate;
using Modules.Users.Domain.ValueObjects;
using StrictId.EFCore;

namespace Modules.Users.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever()
            .HasStrictIdValueGenerator();

        builder.ComplexProperty(u => u.Email);

        builder
            .Property(u => u.Password)
            .HasMaxLength(256)
            .IsRequired();

        builder.ToTable("Users");
    }
}
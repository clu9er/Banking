using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Banking.Domain.Enteties;

namespace Banking.Infrastructure.Persistence.PostgreSQL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder
            .Property(x => x.TotalBalance)
            .HasPrecision(19, 4);
    }
}

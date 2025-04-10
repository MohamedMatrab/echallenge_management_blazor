using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DAL.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(e=>e.UserName).IsRequired();
        builder.Property(e => e.Email).IsRequired();
    }
}

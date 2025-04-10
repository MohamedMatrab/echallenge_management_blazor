using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DAL.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<JwtTokenRefresh>
{
    public void Configure(EntityTypeBuilder<JwtTokenRefresh> builder)
    {
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

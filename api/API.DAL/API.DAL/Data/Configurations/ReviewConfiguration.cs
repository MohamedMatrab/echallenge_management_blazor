using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DAL.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(e=>e.Id);
        builder.HasOne(e => e.Book).WithMany(e => e.Reviews).OnDelete(DeleteBehavior.Cascade).IsRequired();
    }
}
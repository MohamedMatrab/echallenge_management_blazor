using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DAL.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(p => p.Author).IsRequired();
    }
}

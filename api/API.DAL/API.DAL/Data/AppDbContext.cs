using System.Reflection;

namespace API.DAL.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, Role, string>(options)
{
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
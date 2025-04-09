using API.DAL.Entities.Abstractions;

namespace API.DAL.Entities;

public class Review : Entity<Guid>
{
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string ReviewerName { get; set; } = string.Empty;
    public virtual User Reviewer { get; set; } = default!;
    public virtual Book Book { get; set; } = default!;
}

namespace API.DAL.Entities.Abstractions;

public abstract class Entity : IEntity
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}

public abstract class Entity<TKey> : Entity, IEntity<TKey>
{
    public TKey Id { get; set; } = default!;
}
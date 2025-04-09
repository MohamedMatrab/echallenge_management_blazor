namespace API.DAL.Entities.Abstractions;

public interface IEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}

public interface IEntity<TKey> : IEntity
{
    public TKey Id { get; set; }
}
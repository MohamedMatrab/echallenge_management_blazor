using API.DAL.Entities.Abstractions;

namespace API.DAL.Entities;

public class JwtTokenRefresh : Entity<int>
{
    public string? Token { get; set; }
    public string? JwtId { get; set; }
    public bool IsUsed { get; set; }
    public DateTime ExpiredAt { get; set; }

    public string UserId { get; set; } = default!;
    public virtual User User { get; set; } = default!;
}

namespace SharedBlocks.Dtos.Response;

public record AuthResult : Result
{
    public string? Token { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? RefreshToken { get; set; }
}
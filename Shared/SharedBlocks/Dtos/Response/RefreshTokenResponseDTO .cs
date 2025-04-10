namespace SharedBlocks.Dtos.Response;

public class RefreshTokenResponseDto
{
    public string? UserId { get; set; }
    public bool Success { get; set; }
    public List<string> Errors { get; set; } = [];
}
using System.ComponentModel.DataAnnotations;

namespace SharedBlocks.Dtos.Request;

public class TokenRequestDTO
{
    [Required] public string Token { get; set; } = null!;
    [Required] public string RefreshToken { get; set; } = null!;
}
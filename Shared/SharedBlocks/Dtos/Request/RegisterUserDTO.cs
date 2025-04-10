using System.ComponentModel.DataAnnotations;

namespace SharedBlocks.Dtos.Request;

public record RegisterUserDTO
{
    [Required][EmailAddress] public string Email { get; set; } = default!;
    [Required] public string Username { get; set; } = default!;
    [Required] public string Password { get; set; } = default!;
}
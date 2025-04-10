using System.ComponentModel.DataAnnotations;

namespace SharedBlocks.Dtos.Request;

public class LoginUserDTO
{
    [Required] public string UserName { get; set; } = default!;
    [Required] public string Password { get; set; } = default!;
}
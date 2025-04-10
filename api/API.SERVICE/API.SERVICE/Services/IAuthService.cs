using SharedBlocks.Dtos.Request;
using SharedBlocks.Dtos.Response;

namespace API.SERVICE.Services;

public interface IAuthService
{
    Task<AuthResult> Register(RegisterUserDTO user);
    Task<AuthResult> Login(LoginUserDTO user);
    Task<AuthResult> RefreshToken(TokenRequestDTO tokenRequest);
}

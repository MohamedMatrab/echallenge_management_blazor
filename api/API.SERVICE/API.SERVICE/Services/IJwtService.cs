using SharedBlocks.Dtos.Request;
using SharedBlocks.Dtos.Response;

namespace API.SERVICE.Services;

public interface IJwtService
{
    Task<AuthResult> GenerateToken(string userId);
    Task<RefreshTokenResponseDto> VerifyToken(TokenRequestDTO tokenRequest, bool checking, bool validateExp);
}

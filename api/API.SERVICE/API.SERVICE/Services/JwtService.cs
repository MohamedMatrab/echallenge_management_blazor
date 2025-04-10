using API.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SharedBlocks.Dtos.Request;
using SharedBlocks.Dtos.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.SERVICE.Services;

public class JwtService(UserManager<User> userManager) : IJwtService
{
    private readonly UserManager<User> _userManager = userManager;
    public Task<RefreshTokenResponseDTO> VerifyToken(TokenRequestDTO tokenRequest, bool checking, bool validateExp)
    {
        throw new NotImplementedException();
    }

    public async Task<AuthResult> GenerateToken(string userId)
    {
        //return new AuthResult { IsSuccess= false};
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return new AuthResult
            {
                IsSuccess = false,
                Errors = ["User not found"]
            };
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>{
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName??""),
                new(ClaimTypes.Email, user.Email ?? ""),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.SecretKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var expirationDate = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpiryMinutes);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expirationDate,
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                SigningCredentials = signingCredentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            //var refreshToken = new JwtRefreshToken()
            //{
            //    JwtId = token.Id,
            //    IsUsed = false,
            //    IsRevoked = false,
            //    UserId = user.Id,
            //    CreatedAt = DateTime.UtcNow,
            //    ExpiredAt = DateTime.UtcNow.AddMonths(1),
            //    Token = RandomString + Guid.NewGuid()
            //};

            //_dbContext.JwtRefreshTokens.Add(refreshToken);
            //_dbContext.SaveChanges();

            _logger.LogInformation("Token generated for user {UserId} with expiration date {ExpirationDate}", user.Id, expirationDate);
            return jwtToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating token for user {UserId}", user.Id);
            throw;
        }
    }
}
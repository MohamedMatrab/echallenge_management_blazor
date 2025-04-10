using API.DAL.Data;
using API.DAL.Entities;
using API.SERVICE.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedBlocks.Dtos.Request;
using SharedBlocks.Dtos.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.SERVICE.Services.Implementations;

public class JwtService
    (UserManager<User> userManager,
    AppDbContext context,
    TokenValidationParameters tokenValidationParameters,
    IOptionsMonitor<JwtConfig> jwtConfig)
    : IJwtService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly JwtConfig _jwtConfig = jwtConfig.CurrentValue;
    
    public async Task<RefreshTokenResponseDto> VerifyToken(TokenRequestDTO tokenRequest, bool markAsUsedAfterValidation = false, bool validateTokenExpiration = false)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var storedToken = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == tokenRequest.RefreshToken);
            if (storedToken == null)
            {
                return new RefreshTokenResponseDto()
                {
                    Success = false,
                    Errors = ["token not found"]
                };
            }

            var validation = tokenValidationParameters.Clone();
            validation.ValidateLifetime = validateTokenExpiration;
            var tokenVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, validation, out var validatedToken); //?

            ////////////////
            var jti = tokenVerification.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Jti)?.Value;

            if (storedToken.JwtId != jti)
            {
                return new RefreshTokenResponseDto()
                {
                    Success = false,
                    Errors = ["token does not match"]
                };
            }

            // UTC to DateTime
            var expireDate = storedToken.ExpiredAt;

            if (expireDate < DateTime.UtcNow)
            {
                return new RefreshTokenResponseDto()
                {
                    Success = false,
                    Errors = ["Refresh Token is expired"]
                };
            }

            //////////////////
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);//?

                if (!result)
                {
                    return new RefreshTokenResponseDto()
                    {
                        Success = false,
                        Errors = ["Token Algorithm Error!"]
                    };
                }
            }
            //////////////////
            if (storedToken.IsUsed)
            {
                return new RefreshTokenResponseDto()
                {
                    Success = false,
                    Errors = ["token Already used."]
                };
            }

            if (!markAsUsedAfterValidation)
                return new RefreshTokenResponseDto() { Success = true };

            storedToken.IsUsed = true;
            await context.SaveChangesAsync();

            return new RefreshTokenResponseDto()
            {
                UserId = markAsUsedAfterValidation ? null : storedToken.User.Id,
                Success = true
            };
        }
        catch (Exception e)
        {
            return new RefreshTokenResponseDto()
            {
                Errors = [e.Message],
                Success = false
            };
        }

    }
    
    public async Task<AuthResult> GenerateToken(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return new AuthResult
            {
                IsSuccess = false,
                Errors = ["User not found"]
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var claims = new[]
               {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                    new Claim("uid", user.Id)
                }
               .Union(userClaims)
               .Union(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.Secret));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var expirationDate = DateTime.UtcNow.AddMinutes(_jwtConfig.DurationInMinutes);
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

            var refreshToken = new JwtTokenRefresh()
            {
                JwtId = token.Id,
                IsUsed = false,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddMonths(1),
                Token = RandomString + Guid.NewGuid()
            };

            context.RefreshTokens.Add(refreshToken);
            context.SaveChanges();

            return new AuthResult
            {
                Token = jwtToken,
                ExpirationDate = expirationDate,
                RefreshToken = refreshToken.Token,
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new AuthResult 
            {
                Errors = [ex.Message],
                IsSuccess = false
            };
        }
    }

    private string RandomString
    {
        get
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPRSTUVYZWX0123456789";
            return new string(Enumerable.Repeat(chars, 35).Select(n => n[random.Next(n.Length)]).ToArray());
        }
    }
}
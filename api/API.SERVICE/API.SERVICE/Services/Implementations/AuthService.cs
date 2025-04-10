using API.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using SharedBlocks.Dtos.Request;
using SharedBlocks.Dtos.Response;

namespace API.SERVICE.Services.Implementations;

public class AuthService(UserManager<User> userManager,IJwtService jwtService) : IAuthService
{
    public async Task<AuthResult> Register(RegisterUserDTO user)
    {
        if (await userManager.FindByEmailAsync(user.Email) is not null)
            return new AuthResult
            {
                IsSuccess = false,
                Errors = ["Email Exists Already !"]
            };

        if (await userManager.FindByNameAsync(user.Username) is not null)
            return new AuthResult
            {
                IsSuccess = false,
                Errors = ["UserName Exists Already !"]
            };

        var newUser = new User
        {
            Email = user.Email,
            UserName = user.Username
        };

        var created = await userManager.CreateAsync(newUser, user.Password);
        if (!created.Succeeded)
            return new AuthResult()
            {
                Errors = [.. created.Errors.Select(e => e.Description)],
                IsSuccess = false
            };
        var roleResult = await userManager.AddToRoleAsync(newUser, "User");
        if (!roleResult.Succeeded)
        {
            var deleteResult = await userManager.DeleteAsync(newUser);
            return new AuthResult()
            {
                Errors = [.. roleResult.Errors.Select(e => e.Description)],
                IsSuccess = false
            };
        }
        var authResult = await jwtService.GenerateToken(newUser.Id);
        //return a token
        return authResult;

    }
    public async Task<AuthResult> Login(LoginUserDTO user)
    {
        try
        {
            var existingUser = await userManager.FindByNameAsync(user.UserName);

            if (existingUser == null)
                return new AuthResult
                {
                    Errors = ["Email is not registered !"],
                    IsSuccess = false
                };

            var isPasswordCorrect = await userManager.CheckPasswordAsync(existingUser, user.Password);
            if (!isPasswordCorrect)
                return new AuthResult()
                {
                    Errors = ["incorrect password !"],
                    IsSuccess = false
                };

            var authResult = await jwtService.GenerateToken(existingUser.Id);
            return authResult;
        }
        catch (Exception e)
        {
            return new AuthResult
            {
                IsSuccess = false,
                Errors = [e.Message]
            };
        }
    }
    public async Task<AuthResult> RefreshToken(TokenRequestDTO tokenRequest)
    {
        try
        {
            var verified = await jwtService.VerifyToken(tokenRequest, false, false);
            if (!verified.Success)
                return new AuthResult()
                {
                    Errors = verified.Errors,
                    IsSuccess = false
                };

            var tokenUser = await userManager.FindByIdAsync(verified.UserId!);
            if(tokenUser is null)
                return new AuthResult()
                {
                    Errors = ["User not found !"],
                    IsSuccess = false
                };
            var authResult = await jwtService.GenerateToken(tokenUser.Id);
            return authResult;
        }
        catch (Exception e)
        {
            return new AuthResult()
            {
                IsSuccess = false,
                Errors = [e.Message]
            };
        }
    }
}
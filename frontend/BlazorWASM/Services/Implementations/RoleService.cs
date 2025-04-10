using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorWASM.Services.Implementations;

public class RoleService(AuthenticationStateProvider authStateProvider) : IRoleService
{
    private readonly AuthenticationStateProvider _authStateProvider = authStateProvider;

    public async Task<bool> HasRoleAsync(string roleName)
    {
        var roles = await GetUserRolesAsync();
        return roles.Contains(roleName);
    }

    public async Task<bool> HasAnyRoleAsync(params string[] roleNames)
    {
        var roles = await GetUserRolesAsync();
        return roles.Intersect(roleNames).Any();
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        return authState.User.Claims
            .Where(c => IsRoleClaim(c.Type))
            .Select(c => c.Value)
            .Distinct();
    }

    private bool IsRoleClaim(string claimType)
    {
        // Check for various common role claim types
        return claimType == ClaimTypes.Role ||
               claimType == "role" ||
               claimType == "roles" ||
               claimType.EndsWith("/role", StringComparison.OrdinalIgnoreCase);
    }
}
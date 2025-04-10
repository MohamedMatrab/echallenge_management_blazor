namespace BlazorWASM.Services;

public interface IRoleService
{
    Task<bool> HasRoleAsync(string roleName);
    Task<bool> HasAnyRoleAsync(params string[] roleNames);
    Task<IEnumerable<string>> GetUserRolesAsync();
}

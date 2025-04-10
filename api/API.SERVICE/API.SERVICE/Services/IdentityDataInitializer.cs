using API.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace API.Service.Services;

public class IdentityDataInitializer(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IConfiguration configuration) : IIdentityDataInitializer
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly IConfiguration _configuration = configuration;

    public async Task SeedData()
    {
        await SeedRoles();
        await SeedUsers();
    }

    private async Task SeedRoles()
    {
        string[] roleNames = { "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new Role(roleName));
            }
        }
    }

    private async Task SeedUsers()
    {
        var adminEmail = _configuration["DefaultUsers:Admin:Email"] ?? "admin@example.com";
        var adminPassword = _configuration["DefaultUsers:Admin:Password"] ?? "Admin@123!";

        var managerEmail = _configuration["DefaultUsers:Manager:Email"] ?? "manager@example.com";
        var managerPassword = _configuration["DefaultUsers:Manager:Password"] ?? "Manager@123!";

        var userEmail = _configuration["DefaultUsers:User:Email"] ?? "user@example.com";
        var userPassword = _configuration["DefaultUsers:User:Password"] ?? "User@123!";

        await CreateUserIfNotExists(
            email: adminEmail,
            password: adminPassword,
            roles: ["Admin"],
            firstName: "Admin",
            lastName: "User",
            userName: "admin"
        );

        await CreateUserIfNotExists(
            email: managerEmail,
            password: managerPassword,
            roles: ["Manager"],
            firstName: "Manager",
            lastName: "User",
            userName: "manager"
        );

        await CreateUserIfNotExists(
            email: userEmail,
            password: userPassword,
            roles: ["User"],
            firstName: "Regular",
            lastName: "User",
            userName: "user"
        );
    }

    private async Task CreateUserIfNotExists(string email, string password, string[] roles, string userName,string firstName,string lastName)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new User
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true,
                FirstName = firstName,
                LastName = lastName
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                foreach (var role in roles)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
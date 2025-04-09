using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API.DAL.Data.Seeding;

public static class IdentityDataSeeder
{
    public static async Task SeedDefaultUsersAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);

            Console.WriteLine("Successfully seeded default users and roles.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while seeding the database : "+ex.Message);
        }
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        // Define your roles
        string[] roles = { "Admin", "Manager", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private static async Task SeedUsersAsync(UserManager<User> userManager)
    {
        // Admin User
        var adminUser = new User
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "User"
        };

        await CreateUserIfNotExists(userManager, adminUser, "Admin@123!", new[] { "Admin" });

        // Manager User
        var managerUser = new User
        {
            UserName = "manager@example.com",
            Email = "manager@example.com",
            EmailConfirmed = true,
            FirstName = "Manager",
            LastName = "User"
        };

        await CreateUserIfNotExists(userManager, managerUser, "Manager@123!", new[] { "Manager" });

        // Regular User
        var regularUser = new User
        {
            UserName = "user@example.com",
            Email = "user@example.com",
            EmailConfirmed = true,
            FirstName = "Regular",
            LastName = "User"
        };

        await CreateUserIfNotExists(userManager, regularUser, "User@123!", new[] { "User" });
    }

    private static async Task CreateUserIfNotExists(
        UserManager<User> userManager,
        User user,
        string password,
        string[] roles)
    {
        var existingUser = await userManager.FindByEmailAsync(user.Email);

        if (existingUser == null)
        {
            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                foreach (var role in roles)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
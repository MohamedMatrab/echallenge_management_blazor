using API.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace API.SERVICE.Extensions;

public static class DataSeeder
{
    public static async Task UseDataSeeder(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<IIdentityDataInitializer>();
        await initializer.SeedData();
        //return app;
    }
}
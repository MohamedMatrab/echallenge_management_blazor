using API.DAL;
using API.DAL.Data;
using API.DAL.Entities;
using API.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.SERVICE;

public static class DependencyInjection
{
    public static IServiceCollection AddApiService(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDataAccessLayer(configuration);
        services.AddTransient<IIdentityDataInitializer,IdentityDataInitializer>();
        services.AddIdentity<User, Role>(options => { options.SignIn.RequireConfirmedAccount = true; })
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        return services;
    }
}
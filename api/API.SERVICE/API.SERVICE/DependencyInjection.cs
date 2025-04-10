using API.DAL;
using API.DAL.Data;
using API.DAL.Entities;
using API.Service.Services;
using API.SERVICE.Mappers;
using API.SERVICE.Services;
using API.SERVICE.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.SERVICE;

public static class DependencyInjection
{
    public static IServiceCollection AddApiService(this IServiceCollection services,IConfigurationManager configuration)
    {
        services.AddDataAccessLayer(configuration);
        services.AddTransient<IIdentityDataInitializer,IdentityDataInitializer>();
        services.AddIdentity<User, Role>(options => { options.SignIn.RequireConfirmedAccount = true; })
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<IBookService, BookService>();
        services.AddTransient<IJwtService,JwtService>();
        services.AddScoped<IAuthService,AuthService>();
        return services;
    }
}
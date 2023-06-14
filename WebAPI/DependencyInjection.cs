using Application.IServices;
using WebAPI.Services;

namespace WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        // Add DI for IHttpContextAccessor
        services.AddHttpContextAccessor();
        // Add services
        services.AddScoped<IClaimService, ClaimService>();
        services.AddSingleton<ITokenStore, InMemoryTokenStore>();
        services.AddSingleton<IJwtBlacklistService, JwtBlacklistService>();
        return services;
    }
}

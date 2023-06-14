using Application.IServices;
using WebAPI.Services;

namespace WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        // Them CORS cho tat ca moi nguoi deu xai duoc apis
        services.AddCors(options
            => options.AddDefaultPolicy(policy
                => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
        
        // Add DI for IHttpContextAccessor
        services.AddHttpContextAccessor();
        
        // Add services
        services.AddScoped<IClaimService, ClaimService>();
        services.AddSingleton<ITokenStore, InMemoryTokenStore>();
        services.AddSingleton<IJwtBlacklistService, JwtBlacklistService>();
        
        return services;
    }
}

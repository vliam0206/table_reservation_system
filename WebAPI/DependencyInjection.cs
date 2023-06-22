using Application.IServices;
using WebAPI.Services;
using AutoMapper;
using WebAPI.AutoMapper;

namespace WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        //Them CORS cho tat ca moi nguoi deu xai duoc apis
        //services.AddCors(options
        //    => options.AddDefaultPolicy(policy
        //        => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

        // Add CORS configuration
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin() // Allow requests from any origin
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        // Add DI for IHttpContextAccessor
        services.AddHttpContextAccessor();

        // Add auto maper
        services.AddAutoMapper(typeof(MappingProfile));

        // Add services
        services.AddScoped<IClaimService, ClaimService>();
        services.AddSingleton<ITokenStore, InMemoryTokenStore>();
        services.AddSingleton<IJwtBlacklistService, JwtBlacklistService>();
        
        return services;
    }
}

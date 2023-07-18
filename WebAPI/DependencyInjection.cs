using Application.IServices;
using WebAPI.Services;
using AutoMapper;
using WebAPI.AutoMapper;
using Application.Services;

namespace WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
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
        services.AddSingleton<IEmailService, EmailService>();
        
        return services;
    }
}

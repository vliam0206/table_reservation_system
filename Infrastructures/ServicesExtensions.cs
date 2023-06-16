using DataAccess;
using Infrastructures.IRepositories;
using Infrastructures.Repositories;
using Infrastructures.UnitOfWorks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures;

public static class ServicesExtensions
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, 
        string connectionString)
    {        
        services.AddDbContext<AppDBContext>(options =>
                        options.UseSqlServer(connectionString));
        return services;
    }    

    public static IServiceCollection AddJWTConfiguration(this IServiceCollection services, 
        string secretKey)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                { 
                    options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                                    Encoding.UTF8.GetBytes(secretKey))
                        };
                });
        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITableRepository, TableRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IReservationTableRepository, ReservationTableRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}

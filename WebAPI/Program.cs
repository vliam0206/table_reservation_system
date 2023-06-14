using Application.Commons;
using Application.IServices;
using DataAccess;
using Infrastructures;
using System;
using WebAPI;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Bind AppConfiguration from configuration
var config = new AppConfiguration();
builder.Configuration.Bind(config);
builder.Services.AddSingleton(config);

// Add DBContext
builder.Services.AddAppDbContext(config.ConnectionStrings.DefaultDB);
// Add jwt configuration
builder.Services.AddJWTConfiguration(config.JwtConfiguration.SecretKey);
// Add all infrastructure services
builder.Services.AddInfrastructureServices();
// Add all WebApi services
builder.Services.AddWebApiServices();


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Initialize data for DB
SeedDatabase();

//use authentication to use jwt
app.UseAuthentication();
app.UseAuthorization();

// Use routing
app.UseRouting();

app.MapControllers();

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<AppDBContext>();
            //context.Database.EnsureCreated(); // create database if not exist, add table if not has any
            DBInitializer.InitializeData(context);
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred when seeding the DB.");
        }
    }
}

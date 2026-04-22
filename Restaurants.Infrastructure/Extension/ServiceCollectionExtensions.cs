using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Requirments;
using Restaurants.Infrastructure.Authorization.Services;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("RestaurantsDb");
        services.AddDbContext<RestaurantsDbContext>(options => options.UseSqlServer(connectionString)
        .EnableSensitiveDataLogging()
        );
        services.AddIdentityApiEndpoints<User>().AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory< RestaurantsUserClaimsPrincipleFactory>()
            .AddEntityFrameworkStores<RestaurantsDbContext>();//.AddRoles<IdentityRole>(); allows user role management for authorization. Without it, accessing role-protected resources can trigger a 403 Forbidden error.
        services.AddScoped<IRestaurantsSeeder, RestaurantsSeeder>();
        services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
        services.AddScoped<IDishesRepository, DishesRepository>();
        services.AddAuthorizationBuilder()
            //.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality")); //It checks the Nationality column, and if it has a value, the user is authorized to access the endpoint; if the Nationality column is empty, access to the endpoint is denied.
            .AddPolicy(PolicyName.HasNationality, builder => builder.RequireClaim(AppClaimTypes.Nationality, "Indian", "German"))
            .AddPolicy(PolicyName.AtLeast20, builder => builder.AddRequirements(new MinimumAgeRequirment(20)));

        services.AddScoped<IAuthorizationHandler, MinimumAgeRequirmentHandler>();
        services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();
        
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Contracts;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.requirements;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.Infrastructure.Extensions;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{

		services.AddDbContext<RestaurantDbContext>(op =>
		{
			op.UseSqlServer(configuration.GetConnectionString("RestaurantnsDb"));
			op.EnableSensitiveDataLogging();
		});

		services.AddIdentityApiEndpoints<User>(options =>
			{
				options.ClaimsIdentity.SecurityStampClaimType = nameof(User.SecurityStamp);
				options.User.RequireUniqueEmail = true;
			})
			.AddDefaultTokenProviders()
			.AddRoles<IdentityRole>()
			.AddClaimsPrincipalFactory<AppUserClaimsPrincipalFactory>()
			.AddEntityFrameworkStores<RestaurantDbContext>();

		services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();

		services.AddScoped<IAuthorizationHandler, ValidSecurityStampRequirementsHandler>();

		services.AddAuthorizationBuilder()
			//.AddDefaultPolicy("ValidateToken", policy => policy.AddRequirements(new ValidSecurityStampRequirements()))
			.AddPolicy(UserRoles.Admin, policy => policy.RequireRole("Admin"));

		return services;
	}
}
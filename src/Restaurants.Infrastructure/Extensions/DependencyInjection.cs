using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Extensions;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{

		services.AddDbContext<RestaurantsDbContext>(op =>
		{
			op.UseSqlServer(configuration.GetConnectionString("RestaurantnsDb"));
			op.EnableSensitiveDataLogging();
		});

		services.AddIdentityApiEndpoints<User>(options =>
			{
				options.ClaimsIdentity.SecurityStampClaimType = nameof(User.SecurityStamp);
				options.User.RequireUniqueEmail = true;
			})
			.AddRoles<IdentityRole>()
			.AddClaimsPrincipalFactory<AppUserClaimsPrincipalFactory>()
			.AddEntityFrameworkStores<RestaurantsDbContext>();

		services.AddAuthorizationBuilder()
			//.AddDefaultPolicy("ValidateToken", policy => policy.AddRequirements(new ValidSecurityStampRequirements()))
			.AddPolicy(UserRoles.Admin, policy => policy.RequireRole("Admin"));

		return services;
	}
}
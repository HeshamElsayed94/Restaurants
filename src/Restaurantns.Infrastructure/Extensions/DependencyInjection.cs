using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurantns.Application.Contracts;
using Restaurantns.Domain.Constans;
using Restaurantns.Domain.Entities;
using Restaurantns.Infrastructure.Authorization;
using Restaurantns.Infrastructure.Authorization.requirements;
using Restaurantns.Infrastructure.Persistence;
using Restaurantns.Infrastructure.Seeders;

namespace Restaurantns.Infrastructure.Extensions;

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
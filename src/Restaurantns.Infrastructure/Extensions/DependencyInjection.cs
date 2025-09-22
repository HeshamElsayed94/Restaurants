using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurantns.Application.Contracts;
using Restaurantns.Domain.Entities;
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

		services.AddIdentityApiEndpoints<User>()
			.AddRoles<IdentityRole>()
			.AddDefaultTokenProviders()
			.AddEntityFrameworkStores<RestaurantDbContext>();

		services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();

		return services;
	}
}
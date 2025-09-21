using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurantns.Infrastructure.Persistence;
using Restaurantns.Infrastructure.Seeders;

namespace Restaurantns.Infrastructure.Extensions;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{

		services.AddDbContext<RestaurantDbContext>(op => { op.UseSqlServer(configuration.GetConnectionString("RestaurantnsDb")); });

		services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();

		return services;
	}
}
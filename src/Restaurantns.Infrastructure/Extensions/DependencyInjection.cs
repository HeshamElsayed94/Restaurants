using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurantns.Infrastructure.Persistence;

namespace Restaurantns.Infrastructure.Extensions;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{

		services.AddDbContext<RestaurantDbContext>(op => { op.UseSqlServer(configuration.GetConnectionString("RestaurantnsDb")); });

		return services;
	}
}
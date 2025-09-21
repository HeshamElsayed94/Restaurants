using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurantns.Application.Contracts;
using Restaurantns.Application.Restaurants.Commands.CreateRestaurant;
using Restaurantns.Application.Restaurants.Dtos;

namespace Restaurantns.Application.Restaurants;

internal class RestaurantService(IRestaurantsDbContext dbContext, ILogger<RestaurantService> logger) : IRestaurantService
{

	public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants(CancellationToken cancellationToken = default)
	{
		logger.LogInformation("Get all restaurants");

		var restaurants = await dbContext.Restaurants
			.ToListAsync(cancellationToken);

		return RestaurantDto.FromEntity(restaurants);
	}

	public async Task<RestaurantDto?> GetRestaurant(int id, CancellationToken cancellationToken = default)
	{
		logger.LogInformation("Get Restaurant {id}", id);

		var restaurant = await dbContext.Restaurants
			.Include(r => r.Dishes)
			.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

		return RestaurantDto.FromEntity(restaurant);
	}

	public async Task<int> CreateRestaurant(CreateRestaurantCommand command, CancellationToken cancellationToken = default)
	{
		var restaurantEntity = CreateRestaurantCommand.ToEntity(command);

		dbContext.Restaurants.Add(restaurantEntity);

		await dbContext.SaveChangesAsync(cancellationToken);

		return restaurantEntity.Id;
	}
}
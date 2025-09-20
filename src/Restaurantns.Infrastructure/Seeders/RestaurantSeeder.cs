using Restaurantns.Domain.Entites;
using Restaurantns.Infrastructure.Persistence;

namespace Restaurantns.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantDbContext dbContext) : IRestaurantSeeder
{
	public async Task Seed()
	{
		if (await dbContext.Database.CanConnectAsync())
		{
			if (!dbContext.Restaurants.Any())
			{
				var restaurant = GetRestaurants();
				await dbContext.Restaurants.AddRangeAsync(restaurant);
				await dbContext.SaveChangesAsync();
			}
		}
	}

	private List<Restaurant> GetRestaurants() =>
	[
		new()
		{
			Name = "KFC",
			Category = "Fast Food",
			Description = "KFC is an American fast food restaurant.",
			ContactEmail = "contact@kfc.com",
			HasDelivery = true,
			Dishes =
			[
				new()
				{
					Name = "Nashville Hot Chicken",
					Description = "Nashville Hot Chicken (10 pcs.)",
					Price = 10.30m
				},
				new()
				{
					Name = "Chicken Nuggets",
					Description = "Chicken Nuggets (5 pcs.)",
					Price = 5.30m
				}

			]
		}
	];
}
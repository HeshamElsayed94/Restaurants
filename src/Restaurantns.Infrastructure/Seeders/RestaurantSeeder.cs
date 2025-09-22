using Microsoft.AspNetCore.Identity;
using Restaurantns.Application.Contracts;
using Restaurantns.Domain.Constans;
using Restaurantns.Domain.Entities;
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
			}

			if (!dbContext.Roles.Any())
			{
				var role = GetRoles();
				await dbContext.Roles.AddRangeAsync(role);
			}

			await dbContext.SaveChangesAsync();
		}
	}

	private IEnumerable<IdentityRole> GetRoles() =>
	[
		new(UserRoles.User)
		{
			NormalizedName = UserRoles.User.ToUpper()
		},
		new(UserRoles.Admin)
		{
			NormalizedName = UserRoles.Admin.ToUpper()
		},
		new(UserRoles.Owner)
		{
			NormalizedName = UserRoles.Owner.ToUpper()
		}

	];

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
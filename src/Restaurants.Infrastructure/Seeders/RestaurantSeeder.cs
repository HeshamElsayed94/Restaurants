using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Entities.Restaurants;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantsDbContext dbContext, UserManager<User> userManager)
: IRestaurantSeeder
{
	public async Task Seed()
	{
		if (await dbContext.Database.CanConnectAsync())
		{
			await dbContext.Database.MigrateAsync();

			await SeedRoles();

			if (await userManager.GetUsersInRoleAsync(UserRoles.Admin) is { Count: 0 })
			{
				string? adminEmail = Environment.GetEnvironmentVariable("Admin__Email");
				string? adminPassword = Environment.GetEnvironmentVariable("Admin__Password");

				var admin = new User() { Email = adminEmail, UserName = adminEmail };

				await userManager.CreateAsync(admin, adminPassword!);
				await userManager.AddToRoleAsync(admin, UserRoles.Admin);
			}

			if (!dbContext.Restaurants.Any())
			{
				User? owner = null;

				if (!dbContext.Users.Any())
				{

					owner = new User()
					{
						Id = "d4d8e926-9242-4c00-aa32-8c24d8c95231",
						Email = "Owner@test.com",
						UserName = "Owner@test.com"
					};

					await userManager.CreateAsync(owner, "Owner@229");
					await userManager.AddToRoleAsync(owner, UserRoles.Owner);
				}
				else
				{
					var owners = await userManager.GetUsersInRoleAsync(UserRoles.Owner);
					owner = owners.FirstOrDefault();
				}

				if (owner is not null)
				{
					var restaurant = GetRestaurants(owner.Id);
					await dbContext.Restaurants.AddRangeAsync(restaurant);
				}

			}

			await dbContext.SaveChangesAsync();
		}
	}

	private async Task SeedRoles()
	{
		var existingRoleNames = await dbContext.Roles
						.Select(r => r.Name)
						.ToListAsync();

		var missingRoles = GetRoles()
			.Where(r => !existingRoleNames.Contains(r.Name))
			.ToList();

		if (missingRoles.Count != 0)
			await dbContext.Roles.AddRangeAsync(missingRoles);
	}

	private static IEnumerable<IdentityRole> GetRoles()
	=> [
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

	private static List<Restaurant> GetRestaurants(string ownerId) => [
		new()
		{
			Name = "KFC",
			Category = "Fast Food",
			Description = "KFC is an American fast food restaurant.",
			ContactEmail = "contact@kfc.com",
			OwnerId = ownerId,
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
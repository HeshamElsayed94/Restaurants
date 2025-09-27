using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Entities.Restaurants;

namespace Restaurants.Domain.Interfaces;

public interface IRestaurantsDbContext
{
	DbSet<Restaurant> Restaurants { get; }

	DbSet<Dish> Dishes { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Contracts;

public interface IRestaurantsDbContext
{
	DbSet<Restaurant> Restaurants { get; }

	DbSet<Dish> Dishes { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
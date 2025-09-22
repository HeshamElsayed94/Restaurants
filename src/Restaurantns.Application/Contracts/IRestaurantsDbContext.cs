using Microsoft.EntityFrameworkCore;
using Restaurantns.Domain.Entities;

namespace Restaurantns.Application.Contracts;

public interface IRestaurantsDbContext
{
	DbSet<Restaurant> Restaurants { get; }

	DbSet<Dish> Dishes { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
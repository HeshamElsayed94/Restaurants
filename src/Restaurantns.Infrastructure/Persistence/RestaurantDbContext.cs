using Microsoft.EntityFrameworkCore;
using Restaurantns.Application.Contracts;
using Restaurantns.Domain.Entites;

namespace Restaurantns.Infrastructure.Persistence;

internal class RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : DbContext(options), IRestaurantsDbContext
{
	public DbSet<Restaurant> Restaurants { get; set; }

	public DbSet<Dish> Dishes { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Restaurant>()
			.OwnsOne(r => r.Address);

		modelBuilder.Entity<Restaurant>()
			.HasMany(r => r.Dishes)
			.WithOne()
			.HasForeignKey(d => d.RestaurantId);
	}
}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurants.Application.Contracts;
using Restaurants.Domain.Contracts;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Entities.Restaurants;

namespace Restaurants.Infrastructure.Persistence;

internal class RestaurantsDbContext(DbContextOptions<RestaurantsDbContext> options)
: IdentityDbContext<User>(options), IRestaurantsDbContext
{
	public DbSet<Restaurant> Restaurants { get; set; }

	public DbSet<Dish> Dishes { get; set; }

	// public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
	// { var modifiedUsers = ChangeTracker.Entries<IdentityUser>() .Where(u => u.State is
	// EntityState.Modified) .Select(u => u.Entity) .ToList();
	//
	// foreach (var modifiedUser in modifiedUsers) modifiedUser.SecurityStamp = Guid.NewGuid().ToString();
	//
	// var changedUserRoleIds = ChangeTracker.Entries<IdentityUserRole<string>>() .Where(x =>
	// x.State is not EntityState.Unchanged and not EntityState.Detached) .Select(x =>
	// x.Entity.UserId) .Distinct() .ToList();
	//
	// var changedUserClaimIds = ChangeTracker.Entries<IdentityUserClaim<string>>() .Where(x =>
	// x.State is not EntityState.Unchanged and not EntityState.Detached) .Select(x =>
	// x.Entity.UserId) .Distinct() .ToList();
	//
	// var remainingUsersIds = changedUserRoleIds .Union(changedUserClaimIds)
	// .Except(modifiedUsers.Select(u => u.Id)) .ToList();
	//
	// if (remainingUsersIds.Count != 0) { await Users.Where(u => remainingUsersIds.Contains(u.Id))
	// .ExecuteUpdateAsync(u => u.SetProperty(x => x.SecurityStamp, Guid.NewGuid().ToString()),
	// cancellationToken); }
	//
	// return await base.SaveChangesAsync(cancellationToken);
	//
	// }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Restaurant>()
			.OwnsOne(r => r.Address);

		modelBuilder.Entity<Restaurant>()
			.HasMany(r => r.Dishes)
			.WithOne()
			.HasForeignKey(d => d.RestaurantId);

		modelBuilder.Entity<Restaurant>()
		.HasOne(r => r.Owner)
		.WithMany(r => r.Restaurants)
		.HasForeignKey(r => r.OwnerId);

		modelBuilder.Entity<Dish>()
		.Property(d => d.Price)
		.HasPrecision(18, 2);
	}
}
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Contracts;
using Restaurants.Application.Restaurants.Queries.Caching;
using Restaurants.Domain.Common.Results;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Contracts;
using Restaurants.Domain.Entities.Restaurants;

namespace Restaurants.Application.Dishes.Commands.DeleteDishesForRestaurant;

internal class DeleteDishesForRestaurantCommandHandler(ILogger<DeleteDishesForRestaurantCommandHandler> logger,
IRestaurantsDbContext dbContext,
IRestaurantAuthorizationService authorizationService,
HybridCache cache) : IRequestHandler<DeleteDishesForRestaurantCommand, Result<Success>>
{
	public async ValueTask<Result<Success>> Handle(DeleteDishesForRestaurantCommand request, CancellationToken ct)
	{
		logger.LogInformation("Delete dishes for restaurant with id '{Id}.'", request.RestaurantId);

		var restaurant = await dbContext
			.Restaurants
			.Include(x => x.Dishes)
			.FirstOrDefaultAsync(r => r.Id.Equals(request.RestaurantId), ct);

		if (restaurant is null)
		{
			logger.LogWarning("Restaurant with id '{Id}' not found.", request.RestaurantId);
			return RestaurantErrors.RestaurantNotFound(request.RestaurantId);
		}

		bool isAuthorize = authorizationService.Authorize(restaurant, ResourceOperation.Delete);

		if (!isAuthorize)
			return Error.Forbidden(description: $"You are not the owner for the restaurant with id '{request.RestaurantId}' to add new dish.");

		dbContext.Dishes.RemoveRange(restaurant.Dishes);

		await dbContext.SaveChangesAsync(ct);

		await cache.RemoveByTagAsync(RestaurantCachingTags.Single(request.RestaurantId), ct);
		logger.LogInformation("Cache removed");

		return Result.Success;
	}
}
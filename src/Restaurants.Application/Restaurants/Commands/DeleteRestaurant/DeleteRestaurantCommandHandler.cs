using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Queries.Caching;
using Restaurants.Domain.Common.Results;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Contracts;
using Restaurants.Domain.Entities.Restaurants;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(
	IRestaurantsDbContext dbContext,
	ILogger<DeleteRestaurantCommandHandler> logger,
	IRestaurantAuthorizationService restaurantAuthorization,
	HybridCache cache)
	: IRequestHandler<DeleteRestaurantCommand, Result<Success>>
{
	public async ValueTask<Result<Success>> Handle(DeleteRestaurantCommand request, CancellationToken ct)
	{
		logger.LogInformation("Deleting Restaurant with id '{id}'", request.Id);

		var restaurnat = await dbContext.Restaurants
			.FirstOrDefaultAsync(x => x.Id.Equals(request.Id), ct);

		if (restaurnat is null)
		{
			logger.LogWarning("Restaurant with id '{id} not found'", request.Id);

			return RestaurantErrors.RestaurantNotFound(request.Id);
		}

		bool isAuthorize = restaurantAuthorization
			.Authorize(restaurnat, ResourceOperation.Delete);

		if (!isAuthorize)
			return Error.Forbidden(description: $"You are not the owner of the restaurant with id '{request.Id}'.");

		dbContext.Restaurants.Remove(restaurnat);

		await dbContext.SaveChangesAsync(ct);

		await cache.RemoveByTagAsync(RestaurantCachingTags.Single(request.Id), ct);
		logger.LogInformation("Cache removed");

		return Result.Success;
	}
}
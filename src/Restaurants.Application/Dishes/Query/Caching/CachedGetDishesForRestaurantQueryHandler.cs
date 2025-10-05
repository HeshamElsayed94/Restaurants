using Mediator;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common.Results;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Query.GetDishesForRestaurant;
using Restaurants.Application.Restaurants.Queries.Caching;

namespace Restaurants.Application.Dishes.Query.Caching;
internal class CachedGetDishesForRestaurantQueryHandler(ILogger<CachedGetDishesForRestaurantQueryHandler> logger,
IRequestHandler<GetDishesForRestaurantQuery, Result<IEnumerable<DishDto>>> handler,
HybridCache cache) : IRequestHandler<GetDishesForRestaurantQuery, Result<IEnumerable<DishDto>>>
{
	public async ValueTask<Result<IEnumerable<DishDto>>> Handle(GetDishesForRestaurantQuery request, CancellationToken ct)
	{
		logger.LogInformation(
			"Get dishes for restaurant with id '{Id}' from cache.",
			request.RestaurantId);

		var result = await cache.GetOrCreateAsync(
		$"Restaurants{request.RestaurantId}:Dishes",
		async ct =>
		{
			logger.LogInformation("Cache created");
			return await handler.Handle(request, ct);
		}, tags: [RestaurantCachingTags.Main, .. RestaurantCachingTags.Single(request.RestaurantId)], cancellationToken: ct);

		return result;
	}
}
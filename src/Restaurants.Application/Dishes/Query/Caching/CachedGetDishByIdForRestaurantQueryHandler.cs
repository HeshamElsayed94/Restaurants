using Mediator;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Query.GetByIdForRestaurant;
using Restaurants.Application.Restaurants.Queries.Caching;
using Restaurants.Domain.Common.Results;

namespace Restaurants.Application.Dishes.Query.Caching;
public class CachedGetDishByIdForRestaurantQueryHandler(ILogger<CachedGetDishByIdForRestaurantQueryHandler> logger,
HybridCache cache,
IRequestHandler<GetDishByIdForRestaurantQuery, Result<DishDto>> handler)
: IRequestHandler<GetDishByIdForRestaurantQuery, Result<DishDto>>
{
	public async ValueTask<Result<DishDto>> Handle(GetDishByIdForRestaurantQuery request, CancellationToken ct)
	{
		logger.LogInformation(
			"Get dish with id '{Id}' for restaurant with id '{RestaurantId}' from caching.",
			request.Id,
			request.RestaurantId);
		;

		var result = await cache.GetOrCreateAsync($"Restaurants:Dishes:{request.Id}",
		async ct =>
		{
			logger.LogInformation("Cache created");
			return await handler.Handle(request, ct);
		}, tags: [RestaurantCachingTags.Main, .. RestaurantCachingTags.Single(request.RestaurantId)], cancellationToken: ct);

		return result;
	}
}
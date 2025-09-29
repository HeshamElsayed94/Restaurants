using Mediator;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

namespace Restaurants.Application.Restaurants.Queries.Caching;
#pragma warning disable MSG0001 // MediatorGenerator multiple handlers
public class CachedGetAllRestaurantsQueryHandler(
	IRequestHandler<GetAllRestaurantsQuery, PagedList<RestaurantDto>> requestHandler,
	ILogger<CachedGetAllRestaurantsQueryHandler> logger,
	HybridCache cache) : IRequestHandler<GetAllRestaurantsQuery, PagedList<RestaurantDto>>
{
	public async ValueTask<PagedList<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken ct)
	{
		logger.LogInformation("Getting restaurants from caching");

		var result = await cache.GetOrCreateAsync($"Restaurants:{request.PageNumber}:{request.PageSize}",
		async ct =>
		{
			logger.LogInformation("Cache created");
			return await requestHandler.Handle(request, ct);
		}, tags: [RestaurantCachingTags.Main, RestaurantCachingTags.Paged], cancellationToken: ct);

		return result;
	}
}
using Mediator;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common.Results;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

namespace Restaurants.Application.Restaurants.Queries.Caching;
internal class CachedGetRestaurantByIdQueryHandler(
	IRequestHandler<GetRestaurantByIdQuery, Result<RestaurantDto>> requestHandler,
	ILogger<CachedGetRestaurantByIdQueryHandler> logger,
	HybridCache cache) : IRequestHandler<GetRestaurantByIdQuery, Result<RestaurantDto>>
{
	public async ValueTask<Result<RestaurantDto>> Handle(GetRestaurantByIdQuery request, CancellationToken ct)
	{
		logger.LogInformation("Getting restaurant with id '{Id}' from caching.", request.Id);

		var result = await cache.GetOrCreateAsync(
		$"Restaurants:{request.Id}",
		async ct =>
		{
			logger.LogInformation("Cache created");
			return await requestHandler.Handle(request, ct);
		}, tags: [RestaurantCachingTags.Main, .. RestaurantCachingTags.Single(request.Id)], cancellationToken: ct);

		return result;
	}
}
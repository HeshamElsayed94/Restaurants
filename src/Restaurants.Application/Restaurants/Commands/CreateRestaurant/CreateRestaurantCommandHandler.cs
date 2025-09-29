using System.Security.Claims;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Queries.Caching;
using Restaurants.Domain.Common.Results;
using Restaurants.Domain.Contracts;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(
	ILogger<CreateRestaurantCommandHandler> logger,
	IRestaurantsDbContext dbContext,
	IHttpContextAccessor httpAcessor,
	HybridCache cache)
	: IRequestHandler<CreateRestaurantCommand, Result<int>>
{
	public async ValueTask<Result<int>> Handle(CreateRestaurantCommand request, CancellationToken ct)
	{
		string? ownerId = httpAcessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

		logger.LogInformation(
			"Creating new restaurant {Restaurant} for User with id : '{id}'",
			request,
			ownerId);

		var restaurantEntity = CreateRestaurantCommand.ToEntity(request);
		restaurantEntity.OwnerId = ownerId;

		dbContext.Restaurants.Add(restaurantEntity);

		await dbContext.SaveChangesAsync(ct);

		await cache.RemoveByTagAsync(RestaurantCachingTags.Paged, ct);

		return restaurantEntity.Id;
	}
}
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common.Results;
using Restaurants.Application.Common.Results.Errors;
using Restaurants.Application.Contracts;
using Restaurants.Application.Dishes.Extensions;
using Restaurants.Application.Restaurants.Errors;
using Restaurants.Application.Restaurants.Queries.Caching;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Contracts;

namespace Restaurants.Application.Dishes.Commands.CreateDish;
internal class CreateDishCommandHandler(ILogger<CreateDishCommandHandler> logger,
IRestaurantsDbContext dbContext, IRestaurantAuthorizationService authorizationService,
HybridCache cache) : IRequestHandler<CreateDishCommand, Result<int>>
{
	public async ValueTask<Result<int>> Handle(CreateDishCommand request, CancellationToken ct)
	{
		logger.LogInformation("Create new Dish {@DishRequest}", request);

		var restaurant = await dbContext.Restaurants
						.FirstOrDefaultAsync(x => x.Id.Equals(request.RestaurantId), cancellationToken: ct);

		if (restaurant is null)
		{
			logger.LogWarning("Restaurant with id '{Id}' not found.", request.RestaurantId);
			return RestaurantErrors.RestaurantNotFound(request.RestaurantId);
		}

		bool isAuthorize = authorizationService.Authorize(restaurant, ResourceOperation.Create);

		if (!isAuthorize)
			return Error.Forbidden(description: $"You are not the owner for the restaurant with id '{request.RestaurantId}' to add new dish.");

		var dish = request.ToEntity();

		await dbContext.Dishes.AddAsync(dish, ct);

		await dbContext.SaveChangesAsync(ct);

		await cache.RemoveByTagAsync(RestaurantCachingTags.Single(request.RestaurantId), ct);
		logger.LogInformation("Cache removed");

		return dish.Id;
	}
}
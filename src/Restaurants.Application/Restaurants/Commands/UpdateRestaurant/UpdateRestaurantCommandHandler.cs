using Mediator;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common.Results;
using Restaurants.Application.Common.Results.Errors;
using Restaurants.Application.Contracts;
using Restaurants.Application.Restaurants.Commands.Extensions;
using Restaurants.Application.Restaurants.Errors;
using Restaurants.Application.Restaurants.Queries.Caching;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Contracts;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

internal class UpdateRestaurantCommandHandler(
	ILogger<UpdateRestaurantCommandHandler> logger,
	IRestaurantsDbContext dbContext,
	IUserContext userContext,
	HybridCache cache,
	IRestaurantAuthorizationService authorizationService) : IRequestHandler<UpdateRestaurantCommand, Result<Success>>
{

	public async ValueTask<Result<Success>> Handle(UpdateRestaurantCommand request, CancellationToken ct)
	{
		var user = userContext.GetCurrentUser();

		logger.LogInformation("User with email {Email} updating restaurant with id '{Id}'", user!.Email, request.Id);

		var restaurant = dbContext.Restaurants.FirstOrDefault(x => x.Id.Equals(request.Id));

		if (restaurant is null)
		{
			logger.LogWarning("Restaurant with id '{Id}' not found", request.Id);

			return RestaurantErrors.RestaurantNotFound(request.Id);
		}

		bool isAuthorize = authorizationService.Authorize(restaurant, ResourceOperation.Update);

		if (!isAuthorize)
			return Error.Forbidden(description: $"You are not the owner of the restaurant with id '{request.Id}'.");

		restaurant.Update(request);

		await dbContext.SaveChangesAsync(ct);

		await cache.RemoveByTagAsync(RestaurantCachingTags.Single(request.Id), ct);
		logger.LogInformation("Cache removed");

		return Result.Success;
	}
}
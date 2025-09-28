using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Extensions;
using Restaurants.Domain.Common.Results;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Contracts;
using Restaurants.Domain.Entities.Restaurants;

namespace Restaurants.Application.Dishes.Commands.CreateDish;
public class CreateDishCommandHandler(ILogger<CreateDishCommandHandler> logger,
IRestaurantsDbContext dbContext, IRestaurantAuthorizationService authorizationService) : IRequestHandler<CreateDishCommand, Result<int>>
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

		return dish.Id;
	}
}
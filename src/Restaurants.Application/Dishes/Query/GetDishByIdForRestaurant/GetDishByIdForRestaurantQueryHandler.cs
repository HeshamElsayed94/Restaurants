using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common.Results;
using Restaurants.Application.Common.Results.Errors;
using Restaurants.Application.Contracts;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Restaurants.Errors;

namespace Restaurants.Application.Dishes.Query.GetDishByIdForRestaurant;

internal class GetDishByIdForRestaurantQueryHandler(ILogger<GetDishByIdForRestaurantQueryHandler> logger,
IRestaurantsDbContext dbContext) : IRequestHandler<GetDishByIdForRestaurantQuery, Result<DishDto>>
{
	public async ValueTask<Result<DishDto>> Handle(GetDishByIdForRestaurantQuery request, CancellationToken ct)
	{
		logger.LogInformation(
			"Get dish with id '{Id}' for restaurant with id '{RestaurantId}'.",
			request.Id,
			request.RestaurantId);

		var restaurant = await dbContext.Restaurants.Where(r => r.Id.Equals(request.RestaurantId))
			.Include(x => x.Dishes.Where(d => d.Id.Equals(request.Id))).AsNoTracking()
			.FirstOrDefaultAsync(ct);

		if (restaurant is null)
		{
			logger.LogWarning("Restaurant with id '{Id}' not found.", request.RestaurantId);
			return RestaurantErrors.RestaurantNotFound(request.RestaurantId);
		}

		if (restaurant.Dishes.Count == 0)
		{
			logger.LogWarning("Dish with id '{Id}' not found", request.Id);
			return Error.NotFound(description: $"Dish with id '{request.Id}' not found.");
		}

		return DishDto.FromEntity(restaurant.Dishes[0])!;
	}
}
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Common.Results;
using Restaurants.Domain.Contracts;
using Restaurants.Domain.Entities.Restaurants;

namespace Restaurants.Application.Dishes.Query.GetDishesForRestaurant;

public class GetDishesForRestaurantQueryHandler(ILogger<GetDishesForRestaurantQueryHandler> logger,
IRestaurantsDbContext dbContext) : IRequestHandler<GetDishesForRestaurantQuery, Result<IEnumerable<DishDto>>>
{
	public async ValueTask<Result<IEnumerable<DishDto>>> Handle(GetDishesForRestaurantQuery request, CancellationToken ct)
	{
		logger.LogInformation("Get dishes for restaurant with id '{Id}'", request.RestaurantId);

		var restaurant = await dbContext.Restaurants.Where(x => x.Id.Equals(request.RestaurantId))
			.Include(r => r.Dishes).AsNoTracking()
			.FirstOrDefaultAsync(ct);

		if (restaurant is null)
		{
			logger.LogWarning("Restaurant with id '{Id}' not found", request.RestaurantId);
			return RestaurantErrors.RestaurantNotFound(request.RestaurantId);
		}

		var dishsDto = DishDto.FromEntity(restaurant.Dishes)!;

		return dishsDto;
	}
}
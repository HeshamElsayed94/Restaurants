using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Common.Results;
using Restaurants.Domain.Contracts;
using Restaurants.Domain.Entities.Restaurants;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQueryHandler(IRestaurantsDbContext dbContext, ILogger<GetRestaurantByIdQueryHandler> logger)
	: IRequestHandler<GetRestaurantByIdQuery, Result<RestaurantDto>>
{

	public async ValueTask<Result<RestaurantDto>> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Get Restaurant {Id}", request.Id);

		var restaurant = await dbContext.Restaurants
			.Include(r => r.Dishes)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (restaurant is null)
		{
			logger.LogWarning("Restaurant with id {RestaurantId} was not found", request.Id);
			return RestaurantErrors.RestaurantNotFound(request.Id);
		}

		return RestaurantDto.FromEntity(restaurant)!;
	}
}
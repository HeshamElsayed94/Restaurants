using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Contracts;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQueryHandler(IRestaurantsDbContext dbContext, ILogger<GetRestaurantByIdQueryHandler> logger)
	: IRequestHandler<GetRestaurantByIdQuery, RestaurantDto?>
{

	public async Task<RestaurantDto?> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Get Restaurant {Id}", request.Id);

		var restaurant = await dbContext.Restaurants
			.Include(r => r.Dishes)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		return RestaurantDto.FromEntity(restaurant);
	}
}
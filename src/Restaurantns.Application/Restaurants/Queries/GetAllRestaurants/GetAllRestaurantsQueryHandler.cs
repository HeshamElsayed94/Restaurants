using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurantns.Application.Contracts;
using Restaurantns.Application.Restaurants.Dtos;

namespace Restaurantns.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler(IRestaurantsDbContext dbContext, ILogger<GetAllRestaurantsQueryHandler> logger)
	: IRequestHandler<GetAllRestaurantsQuery, IEnumerable<RestaurantDto>>
{
	public async Task<IEnumerable<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Get all restaurants");

		var restaurants = await dbContext.Restaurants
			.AsNoTracking()
			.ToListAsync(cancellationToken);

		return RestaurantDto.FromEntity(restaurants);
	}
}
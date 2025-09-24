using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurantns.Application.Common;
using Restaurantns.Application.Contracts;
using Restaurantns.Application.Restaurants.Dtos;

namespace Restaurantns.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler(IRestaurantsDbContext dbContext, ILogger<GetAllRestaurantsQueryHandler> logger)
	: IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>
{
	public async Task<PagedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Get all restaurants");

		var restaurantsDtoQuery = dbContext.Restaurants
			.Where(x => string.IsNullOrWhiteSpace(request.SearchPhrase) || x.Name.Contains(request.SearchPhrase) || x.Description.Contains(request.SearchPhrase))
			.Select(x => new RestaurantDto(x))
			.AsNoTracking();

		var pagedResult = await PagedResult<RestaurantDto>.Create(restaurantsDtoQuery, request.PageSize, request.PageNumber, cancellationToken);

		return pagedResult;
	}
}
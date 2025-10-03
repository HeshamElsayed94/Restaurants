using System.Linq.Dynamic.Core;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Contracts;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Contracts;
using Restaurants.Domain.Entities.Restaurants;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

internal class GetAllRestaurantsQueryHandler(IRestaurantsDbContext dbContext, ILogger<GetAllRestaurantsQueryHandler> logger)
	: IRequestHandler<GetAllRestaurantsQuery, PagedList<RestaurantDto>>
{
	public async ValueTask<PagedList<RestaurantDto>> Handle(
		GetAllRestaurantsQuery request,
		CancellationToken cancellationToken)
	{
		logger.LogInformation("Get all restaurants");

		var restaurantsDtoQuery = dbContext.Restaurants
			.Where(x => string.IsNullOrWhiteSpace(request.SearchPhrase) || x.Name.Contains(request.SearchPhrase) || x.Description.Contains(request.SearchPhrase))
			.OrderBy($"{request.SortBy ?? nameof(Restaurant.Name)} {request.SortDirection}")
			.Select(x => new RestaurantDto(x))
			.AsNoTracking();

		var pagedResult = await PagedList<RestaurantDto>.Create(restaurantsDtoQuery, request.PageSize, request.PageNumber, cancellationToken);

		return pagedResult;
	}
}
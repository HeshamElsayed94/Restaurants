using System.Linq.Dynamic.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurantns.Application.Contracts;
using Restaurantns.Application.Restaurants.Dtos;
using Restaurantns.Domain.Entities;

namespace Restaurantns.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler(IRestaurantsDbContext dbContext, ILogger<GetAllRestaurantsQueryHandler> logger)
	: IRequestHandler<GetAllRestaurantsQuery, Common.PagedResult<RestaurantDto>>
{
	public async Task<Common.PagedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Get all restaurants");

		var restaurantsDtoQuery = dbContext.Restaurants
			.Where(x => string.IsNullOrWhiteSpace(request.SearchPhrase) || x.Name.Contains(request.SearchPhrase) || x.Description.Contains(request.SearchPhrase))
			.OrderBy($"{request.SortBy ?? nameof(Restaurant.Name)} {request.SortDirection}")
			.Select(x => new RestaurantDto(x))
			.AsNoTracking();

		var pagedResult = await Common.PagedResult<RestaurantDto>.Create(restaurantsDtoQuery, request.PageSize, request.PageNumber, cancellationToken);

		return pagedResult;
	}
}
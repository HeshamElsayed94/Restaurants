using MediatR;
using Restaurantns.Application.Common;
using Restaurantns.Application.Restaurants.Dtos;

namespace Restaurantns.Application.Restaurants.Queries.GetAllRestaurants;

public record GetAllRestaurantsQuery(string? SearchPhrase, int PageNumber, int PageSize, string? SortBy, string SortDirection = "asc") : IRequest<PagedList<RestaurantDto>>;
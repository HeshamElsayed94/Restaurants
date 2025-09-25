using MediatR;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public record GetAllRestaurantsQuery(string? SearchPhrase, int PageNumber, int PageSize, string? SortBy, string SortDirection = "asc") : IRequest<PagedList<RestaurantDto>>;
using MediatR;
using Restaurantns.Application.Restaurants.Dtos;

namespace Restaurantns.Application.Restaurants.Queries.GetAllRestaurants;

public record GetAllRestaurantsQuery : IRequest<IEnumerable<RestaurantDto>>;
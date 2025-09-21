using MediatR;
using Restaurantns.Application.Restaurants.Dtos;

namespace Restaurantns.Application.Restaurants.Queries.GetRestaurantById;

public record GetRestaurantByIdQuery(int Id) : IRequest<RestaurantDto?>;
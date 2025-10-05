using Mediator;
using Restaurants.Application.Common.Results;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public record GetRestaurantByIdQuery(int Id) : IRequest<Result<RestaurantDto>>;
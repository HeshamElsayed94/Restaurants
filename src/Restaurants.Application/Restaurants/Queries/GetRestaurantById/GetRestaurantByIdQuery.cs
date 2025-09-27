using Mediator;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Common.Results;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public record GetRestaurantByIdQuery(int Id) : IRequest<Result<RestaurantDto>>;
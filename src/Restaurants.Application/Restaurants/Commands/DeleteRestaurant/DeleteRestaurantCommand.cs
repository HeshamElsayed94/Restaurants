using Mediator;
using Restaurants.Application.Common.Results;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
public record DeleteRestaurantCommand(int Id) : IRequest<Result<Success>>;
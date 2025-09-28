using Mediator;
using Restaurants.Domain.Common.Results;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
public record DeleteRestaurantCommand(int Id) : IRequest<Result<Success>>;
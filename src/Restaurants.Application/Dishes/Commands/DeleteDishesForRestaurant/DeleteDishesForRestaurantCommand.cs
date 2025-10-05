using Mediator;
using Restaurants.Application.Common.Results;

namespace Restaurants.Application.Dishes.Commands.DeleteDishesForRestaurant;

public record DeleteDishesForRestaurantCommand(int RestaurantId) : IRequest<Result<Success>>;
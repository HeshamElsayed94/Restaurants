using Mediator;
using Restaurants.Domain.Common.Results;

namespace Restaurants.Application.Dishes.Commands.DeleteDishesForRestaurant;

public record DeleteDishesForRestaurantCommand(int RestaurantId) : IRequest<Result<Success>>;
using Mediator;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Common.Results;

namespace Restaurants.Application.Dishes.Query.GetDishesForRestaurant;
public record GetDishesForRestaurantQuery(int RestaurantId) : IRequest<Result<IEnumerable<DishDto>>>;
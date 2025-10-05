using Mediator;
using Restaurants.Application.Common.Results;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Query.GetDishesForRestaurant;
public record GetDishesForRestaurantQuery(int RestaurantId) : IRequest<Result<IEnumerable<DishDto>>>;
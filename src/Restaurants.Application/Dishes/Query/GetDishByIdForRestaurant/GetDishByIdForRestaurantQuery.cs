using Mediator;
using Restaurants.Application.Common.Results;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Query.GetDishByIdForRestaurant;

public record GetDishByIdForRestaurantQuery(int RestaurantId, int Id) : IRequest<Result<DishDto>>;
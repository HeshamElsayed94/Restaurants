using Mediator;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Common.Results;

namespace Restaurants.Application.Dishes.Query.GetDishByIdForRestaurant;

public record GetDishByIdForRestaurantQuery(int RestaurantId, int Id) : IRequest<Result<DishDto>>;
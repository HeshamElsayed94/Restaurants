using Mediator;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Common.Results;

namespace Restaurants.Application.Dishes.Query.GetByIdForRestaurant;

public record GetByIdForRestaurantQuery(int RestaurantId, int Id) : IRequest<Result<DishDto>>;
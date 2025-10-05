using Mediator;
using Restaurants.Application.Common.Results;

namespace Restaurants.Application.Dishes.Commands.CreateDish;
public record CreateDishCommand(
	string Name,
	string Description,
	decimal Price,
	int? KiloCalories,
	int RestaurantId) : IRequest<Result<int>>;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dishes.Extensions;
public static class DishMapper
{

	public static Dish ToEntity(this CreateDishCommand command) => new()
	{
		Name = command.Name,
		Description = command.Description,
		KiloCalories = command.KiloCalories,
		Price = command.Price,
		RestaurantId = command.RestaurantId,
	};

}
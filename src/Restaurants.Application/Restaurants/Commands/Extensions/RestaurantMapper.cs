using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities.Restaurants;

namespace Restaurants.Application.Restaurants.Commands.Extensions;

public static class RestaurantMapper
{
	public static Restaurant Update(this Restaurant restaurant, UpdateRestaurantCommand command)
	{
		restaurant.Name = command.Name;
		restaurant.Description = command.Description;
		restaurant.HasDelivery = command.HasDelivery;

		return restaurant;
	}
}
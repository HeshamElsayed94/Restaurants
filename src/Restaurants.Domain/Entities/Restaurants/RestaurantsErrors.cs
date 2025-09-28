using Restaurants.Domain.Common.Results;

namespace Restaurants.Domain.Entities.Restaurants;
public static class RestaurantErrors
{
	public static Error RestaurantNotFound(int id)
		=> Error.NotFound("Restaurant_NotFound", $"Restaurant with id '{id}' was not found");
}
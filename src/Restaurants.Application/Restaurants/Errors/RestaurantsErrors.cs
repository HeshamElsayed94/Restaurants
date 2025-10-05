using Restaurants.Application.Common.Results.Errors;

namespace Restaurants.Application.Restaurants.Errors;
public static class RestaurantErrors
{
	public static Error RestaurantNotFound(int id)
		=> Error.NotFound("Restaurant_NotFound", $"Restaurant with id '{id}' was not found");
}
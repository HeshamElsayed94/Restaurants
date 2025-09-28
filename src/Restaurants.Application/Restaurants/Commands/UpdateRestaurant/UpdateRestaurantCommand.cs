namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public record UpdateRestaurant(int Id , string Name, string Description, bool HasDelivery);
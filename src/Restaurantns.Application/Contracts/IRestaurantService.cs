using Restaurantns.Application.Restaurants.Commands.CreateRestaurant;
using Restaurantns.Application.Restaurants.Dtos;

namespace Restaurantns.Application.Contracts;

public interface IRestaurantService
{
	Task<IEnumerable<RestaurantDto>> GetAllRestaurants(CancellationToken cancellationToken = default);

	Task<RestaurantDto?> GetRestaurant(int id, CancellationToken cancellationToken = default);

	Task<int> CreateRestaurant(CreateRestaurantCommand command, CancellationToken cancellationToken = default);
}
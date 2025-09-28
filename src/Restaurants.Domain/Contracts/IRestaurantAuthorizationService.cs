using Restaurants.Domain.Constans;
using Restaurants.Domain.Entities.Restaurants;

namespace Restaurants.Domain.Contracts;
public interface IRestaurantAuthorizationService
{
	bool Authorize(Restaurant restaurant, ResourceOperation operation);
}
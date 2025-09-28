using Microsoft.Extensions.Logging;
using Restaurants.Application.Contracts;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Contracts;
using Restaurants.Domain.Entities.Restaurants;

namespace Restaurants.Infrastructure.Authorization.Services;
public class RestaurantAuthorizationService(
	IUserContext userContext,
	ILogger<RestaurantAuthorizationService> logger)
	: IRestaurantAuthorizationService
{
	public bool Authorize(Restaurant restaurant, ResourceOperation operation)
	{
		var user = userContext.GetCurrentUser();
		logger.LogInformation(
			"Authorizing user {UserEmail}, to {Operation} for restaurant {RestaurantName}",
			user!.Email,
			operation,
			restaurant.Name);

		if (user.IsInRole(UserRoles.Admin))
		{
			logger.LogInformation("successful authorization for admin");
			return true;
		}
		if (operation is ResourceOperation.Read or ResourceOperation.Create)
		{
			logger.LogInformation("Create/read operation - successful authorization");
			return true;
		}
		if (operation is ResourceOperation.Update or ResourceOperation.Delete && user.Id == restaurant.OwnerId)
		{
			logger.LogInformation("Update/Delete operation - successful authorization for the owner");
			return true;
		}

		return false;

	}
}
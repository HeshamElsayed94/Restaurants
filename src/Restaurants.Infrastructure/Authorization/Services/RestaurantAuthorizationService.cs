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
			logger.LogInformation("{Operation} operation successful authorization for admin", operation);

			return true;
		}

		if (operation is ResourceOperation.Read or ResourceOperation.Create)
		{
			logger.LogInformation("{Operation} operation - successful authorization", operation);

			return true;
		}

		if (operation is ResourceOperation.Update or ResourceOperation.Delete && user.Id == restaurant.OwnerId)
		{
			logger.LogInformation("{Operation} operation - successful authorization for the owner", operation);

			return true;
		}

		logger.LogWarning("User with email {UserEmail} doesn't have permission to {operation} {RestaurantName}",
			user.Email,
			operation,
			restaurant.Name);

		return false;

	}
}
using System.Security.Claims;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Contracts;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(
	ILogger<CreateRestaurantCommandHandler> logger,
	IRestaurantsDbContext dbContext,
	IHttpContextAccessor httpAcessor)
	: IRequestHandler<CreateRestaurantCommand, int>
{
	public async ValueTask<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
	{
		string? ownerId = httpAcessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

		logger.LogInformation(
			"Creating new restaurant {Restaurant} for User with id : '{id}'",
			request,
			ownerId);

		var restaurantEntity = CreateRestaurantCommand.ToEntity(request);
		restaurantEntity.OwnerId = ownerId;

		dbContext.Restaurants.Add(restaurantEntity);

		await dbContext.SaveChangesAsync(cancellationToken);

		return restaurantEntity.Id;
	}
}
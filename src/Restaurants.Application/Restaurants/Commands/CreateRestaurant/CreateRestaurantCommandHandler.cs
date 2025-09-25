using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Contracts;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> logger, IRestaurantsDbContext dbContext)
	: IRequestHandler<CreateRestaurantCommand, int>
{
	public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Creating new restaurant {Restaurant}", request);

		var restaurantEntity = CreateRestaurantCommand.ToEntity(request);

		dbContext.Restaurants.Add(restaurantEntity);

		await dbContext.SaveChangesAsync(cancellationToken);

		return restaurantEntity.Id;
	}
}
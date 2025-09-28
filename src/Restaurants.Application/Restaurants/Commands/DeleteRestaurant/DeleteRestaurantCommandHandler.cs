using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Common.Results;
using Restaurants.Domain.Contracts;
using Restaurants.Domain.Entities.Restaurants;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
public class DeleteRestaurantCommandHandler(
	IRestaurantsDbContext dbContext,
	ILogger<DeleteRestaurantCommandHandler> logger)
: IRequestHandler<DeleteRestaurantCommand, Result<Success>>
{
	public async ValueTask<Result<Success>> Handle(DeleteRestaurantCommand request, CancellationToken ct)
	{
		logger.LogInformation("Deleting Restaurant with id '{id}'", request.Id);

		var restaurnat = await dbContext.Restaurants
		.FirstOrDefaultAsync(x => x.Id.Equals(request.Id), ct);

		if (restaurnat is null)
		{
			logger.LogWarning("Restaurant with id '{id} not found'", request.Id);
			return RestaurantErrors.RestaurantNotFound(request.Id);
		}

		dbContext.Restaurants.Remove(restaurnat);

		await dbContext.SaveChangesAsync(ct);

		return Result.Success;
	}
}
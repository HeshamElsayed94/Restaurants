using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common.Results;
using Restaurants.Application.Contracts;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Errors;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

internal class GetRestaurantByIdQueryHandler(IRestaurantsDbContext dbContext, ILogger<GetRestaurantByIdQueryHandler> logger)
    : IRequestHandler<GetRestaurantByIdQuery, Result<RestaurantDto>>
{

    public async ValueTask<Result<RestaurantDto>> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Get Restaurant {Id}", request.Id);

        var restaurant = await dbContext.Restaurants
            .Include(r => r.Dishes)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (restaurant is null)
        {
            logger.LogWarning("Restaurant with id {RestaurantId} was not found", request.Id);
            return RestaurantErrors.RestaurantNotFound(request.Id);
        }

        return RestaurantDto.FromEntity(restaurant)!;
    }
}
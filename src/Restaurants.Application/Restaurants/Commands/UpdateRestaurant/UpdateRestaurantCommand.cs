using Mediator;
using Restaurants.Domain.Common.Results;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public record UpdateRestaurantCommand(int Id , string Name, string Description, bool HasDelivery) : IRequest<Result<Success>>;
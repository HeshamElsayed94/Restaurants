using MediatR;
using Restaurantns.Application.Contracts;

namespace Restaurantns.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(IRestaurantService restaurantService) : IRequestHandler<CreateRestaurantCommand, int>
{
	public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
		=> await restaurantService.CreateRestaurant(request, cancellationToken);
}
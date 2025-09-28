using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Query.GetByIdForRestaurant;
using Restaurants.Application.Dishes.Query.GetDishesForRestaurant;
using Restaurants.Domain.Constans;

namespace Restaurants.API.Controllers;

[Route("api/restaurants/{restaurantId}/dishes")]
public class DishesController(IMediator mediator) : ApiController
{

	[HttpGet("{id}")]
	public async Task<IActionResult> GetByIdForRestaurant(
		[FromRoute] int restaurantId,
		[FromRoute] int id,
		CancellationToken ct)
	{
		var result = await mediator.Send(new GetByIdForRestaurantQuery(restaurantId, id), ct);

		return result.Match(Ok, Problem);
	}

	[HttpGet]
	public async Task<IActionResult> GetAllDishesForRestaurant([FromRoute] int restaurantId, CancellationToken ct)
	{
		var result = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId), ct);

		return result.Match(Ok, Problem);
	}

	[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Owner}")]
	[HttpPost]
	public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishCommand command, CancellationToken ct)
	{
		var result = await mediator.Send(command, ct);

		return result.Match(
			id => CreatedAtAction(nameof(GetByIdForRestaurant), new { restaurantId, id }, null),
			Problem);
	}

}
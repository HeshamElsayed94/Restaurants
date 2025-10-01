using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.DeleteDishesForRestaurant;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Query.GetDishByIdForRestaurant;
using Restaurants.Application.Dishes.Query.GetDishesForRestaurant;
using Restaurants.Domain.Constans;

namespace Restaurants.API.Controllers;

[Route("api/restaurants/{restaurantId}/dishes")]
public class DishesController(IMediator mediator) : ApiController
{

	[ProducesResponseType<DishDto>(200)]
	[ProducesResponseType(400)]
	[HttpGet("{id}")]
	public async Task<IActionResult> GetByIdForRestaurant(
			[FromRoute] int restaurantId,
			[FromRoute] int id,
			CancellationToken ct)
	{
		var result = await mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId, id), ct);

		return result.Match(Ok, Problem);
	}

	[ProducesResponseType<IEnumerable<DishDto>>(200)]
	[ProducesResponseType(400)]
	[HttpGet]
	public async Task<IActionResult> GetAllDishesForRestaurant([FromRoute] int restaurantId, CancellationToken ct)
	{
		var result = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId), ct);

		return result.Match(Ok, Problem);
	}

	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Owner}")]
	[HttpPost]
	public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishCommand command, CancellationToken ct)
	{
		var result = await mediator.Send(command, ct);

		return result.Match(
			id => CreatedAtAction(nameof(GetByIdForRestaurant), new { restaurantId, id }, null),
			Problem);
	}

	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Owner}")]
	[HttpDelete]
	public async Task<IActionResult> DeleteDishesForRestaurant([FromRoute] int restaurantId, CancellationToken ct)
	{
		var result = await mediator.Send(
			new DeleteDishesForRestaurantCommand(restaurantId),
			ct);

		return result.Match(_ => NoContent(), Problem);
	}
}
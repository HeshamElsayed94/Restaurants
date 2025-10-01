using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Domain.Constans;

namespace Restaurants.API.Controllers;

[Route("api/restaurants")]
public class RestaurantsController(IMediator mediator) : ApiController
{
	[ProducesResponseType<RestaurantDto>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
	[HttpGet]
	public async Task<IActionResult> GetAllPaged([FromQuery] GetAllRestaurantsQuery query, CancellationToken ct)
		=> Ok(await mediator.Send(query, ct));

	[ProducesResponseType<RestaurantDto>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[HttpGet("{id}")]
	public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken ct)
	{
		var result = await mediator.Send(new GetRestaurantByIdQuery(id), ct);

		return result.Match(Ok, Problem);
	}

	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Owner}")]
	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateRestaurantCommand command, CancellationToken ct)
	{
		var result = await mediator.Send(command, ct);

		return result.Match(
			id => CreatedAtAction(nameof(GetById), new { id }, null),
			Problem);
	}

	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Owner}")]
	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
	{
		var result = await mediator.Send(new DeleteRestaurantCommand(id), ct);

		return result.Match(_ => NoContent(), Problem);
	}

	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Owner}")]
	[HttpPut("{id}")]
	public async Task<IActionResult> Update([FromBody] UpdateRestaurantCommand command, CancellationToken ct)
	{
		var result = await mediator.Send(command, ct);

		return result.Match(_ => NoContent(), Problem);
	}
}
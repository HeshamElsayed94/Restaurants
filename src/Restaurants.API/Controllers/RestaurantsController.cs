using Mediator;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetAll([FromQuery] GetAllRestaurantsQuery query, CancellationToken ct) => Ok(await mediator.Send(query, ct));

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken ct)
	{
		var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id), ct);

		if (restaurant is null)
			return NotFound();

		return Ok(restaurant);

	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateRestaurantCommand command, CancellationToken ct)
	{
		var createdRestaurantId = await mediator.Send(command, ct);

		return CreatedAtAction(nameof(GetById), new { id = createdRestaurantId }, null);

	}
}
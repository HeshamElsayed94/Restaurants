using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurantns.Application.Restaurants.Commands.CreateRestaurant;
using Restaurantns.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurantns.Application.Restaurants.Queries.GetRestaurantById;

namespace Restaurantns.API.Controllers;

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
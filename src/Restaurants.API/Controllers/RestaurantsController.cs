using Mediator;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

namespace Restaurants.API.Controllers;

[Route("api/restaurants")]
public class RestaurantsController(IMediator mediator) : ApiController
{
	[HttpGet]
	public async Task<IActionResult> GetAll([FromQuery] GetAllRestaurantsQuery query, CancellationToken ct) => Ok(await mediator.Send(query, ct));

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken ct)
	{
		var result = await mediator.Send(new GetRestaurantByIdQuery(id), ct);

		return result.Match(Ok,
		Problem
		);

	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateRestaurantCommand command, CancellationToken ct)
	{
		int createdRestaurantId = await mediator.Send(command, ct);

		return CreatedAtAction(nameof(GetById), new { id = createdRestaurantId }, null);

	}
}
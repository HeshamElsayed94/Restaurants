using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurantns.Application.Contracts;
using Restaurantns.Application.Restaurants.Commands.CreateRestaurant;

namespace Restaurantns.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(IRestaurantService restaurantService, IMediator mediator) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetAll(CancellationToken ct) => Ok(await restaurantService.GetAllRestaurants(ct));

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id, CancellationToken ct)
	{
		var restaurant = await restaurantService.GetRestaurant(id, ct);

		if (restaurant is null) return NotFound();

		return Ok(restaurant);

	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateRestaurantCommand command, CancellationToken ct)
	{
		var createdRestaurantId = await mediator.Send(command, ct);

		return CreatedAtAction(nameof(GetById), new { id = createdRestaurantId }, null);

	}
}
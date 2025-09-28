using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Domain.Constans;

namespace Restaurants.API.Controllers;

[Route("api/restaurants/{restaurantId}/dishes")]
public class DishesController(IMediator mediator) : ApiController
{

	//[HttpGet]
	//public IActionResult GetAll(){

	//}

	[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Owner}")]
	[HttpPost]
	public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishCommand command, CancellationToken ct)
	{
		var result = await mediator.Send(command, ct);

		return result.Match(_ => Created(), Problem);
	}

}
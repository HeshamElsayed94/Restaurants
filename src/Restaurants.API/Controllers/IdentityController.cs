using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Contracts;
using Restaurants.Application.Users;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Application.Users.Commands.LogOutFromAllDevices;
using Restaurants.Application.Users.Commands.UnAssignUserRole;
using Restaurants.Domain.Constans;

namespace Restaurants.API.Controllers;

[Route("api/identity")]
public class IdentityController(IUserContext userContext, IMediator mediator) : ApiController
{
	[ProducesResponseType<CurrentUser>(200)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[Authorize]
	[HttpGet("Profile")]
	public IActionResult GetProfile() => Ok(userContext.GetCurrentUser());

	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	[Authorize(Roles = UserRoles.Admin)]
	[HttpPost("userRole")]
	public async Task<IActionResult> AssignUserRole([FromBody] AssignUserRoleCommand command, CancellationToken ct)
	{
		var result = await mediator.Send(command, ct);

		return result.Match(_ => NoContent(), Problem);
	}

	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	[Authorize(Roles = UserRoles.Admin)]
	[HttpDelete("userRole")]
	public async Task<IActionResult> UnAssignUserRole([FromBody] UnAssignUserRoleCommand command, CancellationToken ct)
	{
		var result = await mediator.Send(command, ct);

		return result.Match(_ => NoContent(), Problem);
	}

	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[Authorize]
	[HttpPost("LogOutFromAllDevices")]
	public async Task<IActionResult> LogOutFromAllDevices(CancellationToken ct)
	{
		await mediator.Send(new LogOutFromAllDevicesCommand(), ct);

		return NoContent();
	}

}
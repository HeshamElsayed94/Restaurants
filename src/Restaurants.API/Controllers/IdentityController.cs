using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Contracts;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Application.Users.Commands.LogOutFromAllDevices;
using Restaurants.Application.Users.Commands.UnAssignUserRole;
using Restaurants.Domain.Constans;

namespace Restaurants.API.Controllers;

[Route("api/identity")]
public class IdentityController(IUserContext userContext, IMediator mediator) : ApiController
{
	[Authorize]
	[HttpGet("Profile")]
	public IActionResult GetProfile() => Ok(userContext.GetCurrentUser());

	[Authorize(Roles = UserRoles.Admin)]
	[HttpPost("userRole")]
	public async Task<IActionResult> AssignUserRole([FromBody] AssignUserRoleCommand command, CancellationToken ct)
	{
		bool assignedRole = await mediator.Send(command, ct);

		if (assignedRole)
			return NoContent();

		return BadRequest();
	}

	[Authorize(Roles = UserRoles.Admin)]
	[HttpDelete("userRole")]
	public async Task<IActionResult> UnAssignUserRole([FromBody] UnAssignUserRoleCommand command, CancellationToken ct)
	{
		bool assignedRole = await mediator.Send(command, ct);

		if (assignedRole)
			return NoContent();

		return BadRequest();
	}

	[Authorize]
	[HttpPost("LogOutFromAllDevices")]
	public async Task<IActionResult> LogOutFromAllDevices(CancellationToken ct)
	{
		await mediator.Send(new LogOutFromAllDevicesCommand(), ct);

		return NoContent();
	}

}
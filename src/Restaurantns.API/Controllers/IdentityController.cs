using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurantns.Application.Contracts;
using Restaurantns.Application.Users.Commands.AssignUserRole;
using Restaurantns.Application.Users.Commands.UnAssignUserRole;
using Restaurantns.Domain.Constans;

namespace Restaurantns.API.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(IUserContext userContext, IMediator mediator) : ControllerBase
{
	[Authorize]
	[HttpGet("Profile")]
	public IActionResult GetProfile() => Ok(userContext.GetCurrentUser());

	[Authorize(Roles = UserRoles.Admin)]
	[HttpPost("userRole")]
	public async Task<IActionResult> AssignUserRole([FromBody] AssignUserRoleCommand command, CancellationToken ct)
	{
		var assignedRole = await mediator.Send(command, ct);

		if (assignedRole)
			return NoContent();

		return BadRequest();
	}

	[Authorize(Roles = UserRoles.Admin)]
	[HttpDelete("userRole")]
	public async Task<IActionResult> UnAssignUserRole([FromBody] UnAssignUserRoleCommand command, CancellationToken ct)
	{
		var assignedRole = await mediator.Send(command, ct);

		if (assignedRole)
			return NoContent();

		return BadRequest();
	}
}
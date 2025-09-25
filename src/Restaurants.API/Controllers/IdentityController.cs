using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Contracts;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Application.Users.Commands.UnAssignUserRole;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Entities;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(IUserContext userContext, IMediator mediator, SignInManager<User> signInManager) : ControllerBase
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
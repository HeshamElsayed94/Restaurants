using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurantns.Application.Contracts;
using Restaurantns.Application.Users.Commands.AssignUserRole;
using Restaurantns.Application.Users.Commands.UnAssignUserRole;
using Restaurantns.Domain.Constans;
using Restaurantns.Domain.Entities;

namespace Restaurantns.API.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(IUserContext userContext, IMediator mediator, SignInManager<User> signInManager) : ControllerBase
{
	[Authorize]
	[Authorize(Roles = UserRoles.Admin)]
	[HttpGet("Profile")]
	public IActionResult GetProfile() => Ok(userContext.GetCurrentUser());

	[Authorize]
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

	[Authorize]
	[HttpPost("Logout")]
	public async Task<IActionResult> UnAssignUserRole(CancellationToken ct)
	{
		await signInManager.SignOutAsync();

		return NoContent();
	}
}
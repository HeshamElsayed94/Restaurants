using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Users.Commands.UnAssignUserRole;

public class UnAssignUserRoleCommandHandler(
	ILogger<AssignUserRoleCommandHandler> logger,
	UserManager<User> userManager,
	RoleManager<IdentityRole> roleManager) : IRequestHandler<UnAssignUserRoleCommand, bool>
{

	public async Task<bool> Handle(UnAssignUserRoleCommand request, CancellationToken cancellationToken)
	{
		logger.LogInformation("UnAssign user role : {request}", request);

		var user = await userManager.FindByEmailAsync(request.UserEmail);

		if (user is null) return false;

		var roleExists = await roleManager.RoleExistsAsync(request.RoleName);

		if (!roleExists) return false;

		await userManager.RemoveFromRoleAsync(user, request.RoleName);

		return true;
	}
}
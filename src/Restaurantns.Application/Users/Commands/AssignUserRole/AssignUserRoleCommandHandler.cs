using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurantns.Domain.Entities;

namespace Restaurantns.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommandHandler(
	ILogger<AssignUserRoleCommandHandler> logger,
	UserManager<User> userManager,
	RoleManager<IdentityRole> roleManager) : IRequestHandler<AssignUserRoleCommand, bool>
{

	public async Task<bool> Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Assign user role : {request}", request);

		var user = await userManager.FindByEmailAsync(request.UserEmail);

		if (user is null) return false;

		var roleExists = await roleManager.RoleExistsAsync(request.RoleName);

		if (!roleExists) return false;

		await userManager.AddToRoleAsync(user, request.RoleName);

		return true;
	}
}
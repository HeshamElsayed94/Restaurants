using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommandHandler(
	ILogger<AssignUserRoleCommandHandler> logger,
	UserManager<User> userManager,
	RoleManager<IdentityRole> roleManager) : IRequestHandler<AssignUserRoleCommand, bool>
{

	public async ValueTask<bool> Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
	{
		logger.LogInformation("Assign user role : {request}", request);

		var user = await userManager.FindByEmailAsync(request.UserEmail);

		if (user is null)
			return false;

		user.SecurityStamp = Guid.NewGuid().ToString();

		var securityStampUpdated = await userManager.UpdateAsync(user);

		if (!securityStampUpdated.Succeeded)
			return false;

		var roleExists = await roleManager.RoleExistsAsync(request.RoleName);

		if (!roleExists)
			return false;

		await userManager.AddToRoleAsync(user, request.RoleName);

		return true;
	}
}
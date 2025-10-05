using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common.Results;
using Restaurants.Application.Common.Results.Errors;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Users.Commands.UnAssignUserRole;

internal class UnAssignUserRoleCommandHandler(
	ILogger<AssignUserRoleCommandHandler> logger,
	UserManager<User> userManager,
	RoleManager<IdentityRole> roleManager,
	HybridCache cache) : IRequestHandler<UnAssignUserRoleCommand, Result<Success>>
{

	public async ValueTask<Result<Success>> Handle(UnAssignUserRoleCommand request, CancellationToken ct)
	{
		logger.LogInformation("UnAssign user role : {request}", request);

		var user = await userManager.FindByEmailAsync(request.UserEmail);

		if (user is null)
		{
			logger.LogWarning("User with email {UserEmail} not found.", request.UserEmail);
			return Error.NotFound(description: $"User with email {request.UserEmail} not found.");
		}

		bool roleExists = await roleManager.RoleExistsAsync(request.RoleName);

		if (!roleExists)
		{
			logger.LogWarning("Role {RoleName} not found.", request.RoleName);
			return Error.NotFound(description: $"Role {request.RoleName} not found.");
		}

		await userManager.RemoveFromRoleAsync(user, request.RoleName);

		var securityStampUpdated = await userManager.UpdateSecurityStampAsync(user);

		if (!securityStampUpdated.Succeeded)
		{
			logger.LogError("Update user security stamp failed.");
			await userManager.AddToRoleAsync(user, request.RoleName);
			return Error.Failure(description: "Unassigning to role failed due to server error");
		}

		await cache.RemoveAsync($"Users:{user.Id}", ct);
		logger.LogInformation("Cache removed");

		return Result.Success;
	}
}
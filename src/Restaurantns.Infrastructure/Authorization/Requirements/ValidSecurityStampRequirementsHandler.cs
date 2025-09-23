using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurantns.Domain.Entities;

namespace Restaurantns.Infrastructure.Authorization.requirements;

public class ValidSecurityStampRequirementsHandler(ILogger<ValidSecurityStampRequirementsHandler> logger, UserManager<User> userManager)
	: AuthorizationHandler<ValidSecurityStampRequirements>
{

	protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidSecurityStampRequirements requirement)
	{

		logger.LogInformation("validate token securityStamp for  user : {name} ", context.User.Identity?.Name);

		string? userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (userId is null)
		{
			logger.LogWarning("User is not authenticated");
			context.Fail();

			return;
		}

		string tokenSecurityStamp = context.User.Claims.FirstOrDefault(c => c.Type == nameof(User.SecurityStamp))?.Value!;

		string? userSecurityStamp = await userManager.Users.Where(x => x.Id == userId)
			.Select(x => x.SecurityStamp)
			.FirstOrDefaultAsync();

		if (string.Equals(tokenSecurityStamp, userSecurityStamp, StringComparison.InvariantCultureIgnoreCase))
		{
			logger.LogInformation("Token security stamp is valid");
			context.Succeed(requirement);

			return;
		}

		logger.LogInformation("token security stamp is invalid");
		context.Fail();
	}
}
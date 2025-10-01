using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Restaurants.Domain.Entities;

namespace Restaurants.API.Middlewares;

public class ValidateTokenSecurityStampMiddleware(RequestDelegate next)
{

	public async Task InvokeAsync(HttpContext context, ILogger<ValidateTokenSecurityStampMiddleware> logger,
		UserManager<User> userManager, HybridCache cache)
	{

		logger.LogInformation("validate token securityStamp for  user : {name} ", context.User.Identity!.Name);

		var endpoint = context.GetEndpoint();

		if (endpoint?.Metadata.GetMetadata<IAuthorizeData>() is null)
		{
			logger.LogInformation("endpoint not authorized");

			await next(context);

			return;
		}

		if (context.User.Identity.IsAuthenticated)
		{
			string? userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

			string tokenSecurityStamp = context.User.Claims.FirstOrDefault(c => c.Type == nameof(User.SecurityStamp))!.Value;

			string? userSecurityStamp = await cache.GetOrCreateAsync(
				$"Users:{userId}",
				async ct =>
				{
					logger.LogInformation("Cache created");
					return await userManager
										.Users
										.Where(x => x.Id == userId)
										.Select(x => x.SecurityStamp)
										.FirstOrDefaultAsync(ct);
				},
				tags: ["Users", $"Users:{userId}"]);

			if (string.Equals(tokenSecurityStamp, userSecurityStamp, StringComparison.InvariantCultureIgnoreCase))
			{
				logger.LogInformation("user : {id} security stamp is valid", userId);
				await next(context);
			}
			else
			{
				logger.LogInformation("token security stamp is invalid");

				context.Response.StatusCode = StatusCodes.Status401Unauthorized;

				await context.Response.WriteAsJsonAsync("Token is no longer valid");
			}
		}
		else
		{
			logger.LogInformation("invalid token");
			await next(context);
		}

	}
}
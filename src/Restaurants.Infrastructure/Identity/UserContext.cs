using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Restaurants.Application.Contracts;
using Restaurants.Application.Users;

namespace Restaurants.Infrastructure.Identity;

public class UserContext(IHttpContextAccessor contextAccessor) : IUserContext
{
	public CurrentUser? GetCurrentUser()
	{
		var user = (contextAccessor.HttpContext?.User)
			?? throw new InvalidOperationException("User is null");

		if (user.Identity is null || !user.Identity.IsAuthenticated)
			return null;

		string userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;

		string userEmail = user.FindFirstValue(ClaimTypes.Email)!;

		var roles = user.FindAll(ClaimTypes.Role).Select(r => r.Value);

		return new(userId, userEmail, roles);
	}
}
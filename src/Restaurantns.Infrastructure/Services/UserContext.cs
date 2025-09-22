using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Restaurantns.Application.Contracts;
using Restaurantns.Application.Users;

namespace Restaurantns.Infrastructure.Services;

public class UserContext(IHttpContextAccessor contextAccessor) : IUserContext
{
	public CurrentUser? GetCurrentUser()
	{
		var user = contextAccessor.HttpContext?.User;

		if (user is null)
			throw new InvalidOperationException("User is null");

		if (user.Identity is null || !user.Identity.IsAuthenticated) return null;

		var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;
		var userEmail = user.FindFirstValue(ClaimTypes.Email)!;
		var roles = user.FindAll(ClaimTypes.Role).Select(r => r.Value);

		return new(userId, userEmail, roles);
	}
}
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Restaurantns.Domain.Constans;
using Restaurantns.Domain.Entities;

namespace Restaurantns.Infrastructure.Authorization;

public class AppUserClaimsPrincipalFactory(
	UserManager<User> userManager,
	RoleManager<IdentityRole> roleManager,
	IOptions<IdentityOptions> options)
	: UserClaimsPrincipalFactory<User, IdentityRole>(userManager, roleManager, options)
{
	public override async Task<ClaimsPrincipal> CreateAsync(User user)
	{

		var identity = await GenerateClaimsAsync(user);

		await UserManager.AddToRoleAsync(user, UserRoles.User);

		identity.AddClaim(new(ClaimTypes.Actor, user.UserName ?? ""));

		return new(identity);
	}
}
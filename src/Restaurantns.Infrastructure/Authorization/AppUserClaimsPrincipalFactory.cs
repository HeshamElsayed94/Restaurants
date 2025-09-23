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

		if (identity.Claims.All(x => x.Type != ClaimTypes.Role))
		{
			user.SecurityStamp = Guid.NewGuid().ToString();

			await UserManager.AddToRoleAsync(user, UserRoles.User);

			identity.AddClaim(new(ClaimTypes.Role, UserRoles.User));

			UpdateSecurityStampClaim(user, identity);

		}

		return new(identity);
	}

	private static void UpdateSecurityStampClaim(User user, ClaimsIdentity identity)
	{

		var identitySecurityStamp = identity.Claims.FirstOrDefault(x => x.Type == nameof(User.SecurityStamp));

		identity.RemoveClaim(identitySecurityStamp);

		identity.AddClaim(new(nameof(User.SecurityStamp), user.SecurityStamp!));
	}
}
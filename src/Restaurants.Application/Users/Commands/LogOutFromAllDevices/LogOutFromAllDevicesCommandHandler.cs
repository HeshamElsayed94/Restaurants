using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Contracts;
using Restaurants.Domain.Common.Results;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Users.Commands.LogOutFromAllDevices;
internal class LogOutFromAllDevicesCommandHandler(
	ILogger<LogOutFromAllDevicesCommandHandler> logger,
	IUserContext userContext,
	UserManager<User> userManager,
	HybridCache cache) : IRequestHandler<LogOutFromAllDevicesCommand, Success>
{
	public async ValueTask<Success> Handle(LogOutFromAllDevicesCommand request, CancellationToken ct)
	{
		var userData = userContext.GetCurrentUser();
		logger.LogInformation("Logging out from all devices to user with email {UserEmail}.", userData!.Email);

		var user = await userManager.FindByIdAsync(userData.Id);

		await userManager.UpdateSecurityStampAsync(user!);

		await cache.RemoveAsync($"Users:{user!.Id}", ct);
		logger.LogInformation("Cache removed");

		return Result.Success;
	}
}
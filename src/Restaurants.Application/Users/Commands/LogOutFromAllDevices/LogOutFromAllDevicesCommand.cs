using Mediator;
using Restaurants.Application.Common.Results;

namespace Restaurants.Application.Users.Commands.LogOutFromAllDevices;
public record LogOutFromAllDevicesCommand : IRequest<Success>;
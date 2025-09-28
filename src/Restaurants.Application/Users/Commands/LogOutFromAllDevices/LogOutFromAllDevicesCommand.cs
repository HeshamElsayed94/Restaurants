using Mediator;
using Restaurants.Domain.Common.Results;

namespace Restaurants.Application.Users.Commands.LogOutFromAllDevices;
public record LogOutFromAllDevicesCommand : IRequest<Success>;
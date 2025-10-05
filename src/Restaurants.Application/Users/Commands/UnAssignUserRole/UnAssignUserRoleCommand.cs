using Mediator;
using Restaurants.Application.Common.Results;

namespace Restaurants.Application.Users.Commands.UnAssignUserRole;

public record UnAssignUserRoleCommand(string UserEmail, string RoleName) : IRequest<Result<Success>>;
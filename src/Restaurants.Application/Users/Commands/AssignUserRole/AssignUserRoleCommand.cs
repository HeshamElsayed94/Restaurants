using Mediator;
using Restaurants.Application.Common.Results;

namespace Restaurants.Application.Users.Commands.AssignUserRole;

public record AssignUserRoleCommand(string UserEmail, string RoleName) : IRequest<Result<Success>>;
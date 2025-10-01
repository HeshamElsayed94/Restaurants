using Mediator;
using Restaurants.Domain.Common.Results;

namespace Restaurants.Application.Users.Commands.UnAssignUserRole;

public record UnAssignUserRoleCommand(string UserEmail, string RoleName) : IRequest<Result<Success>>;
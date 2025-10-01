using Mediator;
using Restaurants.Domain.Common.Results;

namespace Restaurants.Application.Users.Commands.AssignUserRole;

public record AssignUserRoleCommand(string UserEmail, string RoleName) : IRequest<Result<Success>>;
using Mediator;

namespace Restaurants.Application.Users.Commands.AssignUserRole;

public record AssignUserRoleCommand(string UserEmail, string RoleName) : IRequest<bool>;
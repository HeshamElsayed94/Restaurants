using MediatR;

namespace Restaurants.Application.Users.Commands.UnAssignUserRole;

public record UnAssignUserRoleCommand(string UserEmail, string RoleName) : IRequest<bool>;
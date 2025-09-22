using MediatR;

namespace Restaurantns.Application.Users.Commands.UnAssignUserRole;

public record UnAssignUserRoleCommand(string UserEmail, string RoleName) : IRequest<bool>;
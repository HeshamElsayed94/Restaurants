using Restaurantns.Application.Users;

namespace Restaurantns.Application.Contracts;

public interface IUserContext
{
	CurrentUser? GetCurrentUser();
}
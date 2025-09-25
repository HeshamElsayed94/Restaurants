using Restaurants.Application.Users;

namespace Restaurants.Application.Contracts;

public interface IUserContext
{
	CurrentUser? GetCurrentUser();
}
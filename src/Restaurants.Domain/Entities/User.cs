using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Entities.Restaurants;

namespace Restaurants.Domain.Entities;

public class User : IdentityUser
{
	public List<Restaurant> Restaurants { get; set; } = [];
}
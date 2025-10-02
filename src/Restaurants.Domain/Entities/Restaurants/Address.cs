namespace Restaurants.Domain.Entities.Restaurants;

public class Address
{
	public string City { get; set; } = null!;

	public string Street { get; set; } = null!;

	public string? PostalCode { get; set; }
}
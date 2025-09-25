namespace Restaurants.Domain.Entities;

public class Dish
{
	public int Id { get; set; }

	public string Name { get; set; } = null!;

	public string Description { get; set; } = null!;

	public decimal Price { get; set; }

	public int? KiloCalories { get; set; }

	public int RestaurantId { get; set; }
}
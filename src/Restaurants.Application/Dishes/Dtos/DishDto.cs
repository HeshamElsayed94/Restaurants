using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dishes.Dtos;

public record DishDto
{
	private DishDto()
	{
	}

	public int Id { get; init; }

	public string Name { get; init; }

	public string Description { get; init; }

	public decimal Price { get; init; }

	public int? KiloCalories { get; init; }

	public static DishDto? FromEntity(Dish? dish)
	{
		if (dish is null) return null;

		return new()
		{

			Id = dish.Id,
			Name = dish.Name,
			Description = dish.Description,
			Price = dish.Price,
			KiloCalories = dish.KiloCalories
		};
	}

	public static List<DishDto?>? FromEntity(List<Dish?>? dishes) => dishes?.Select(FromEntity).ToList();
}
using System.Text.Json.Serialization;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities.Restaurants;

namespace Restaurants.Application.Restaurants.Dtos;

public record RestaurantDto
{

	public RestaurantDto(Restaurant restaurant)
	: this(restaurant.Id, restaurant.Name, restaurant.Description, restaurant.Category, restaurant.HasDelivery, restaurant.Address?.City, restaurant.Address?.Street, restaurant.Address?.PostalCode)
	{

	}

	[JsonConstructor]
	public RestaurantDto(
		int id,
		string name,
		string description,
		string category,
		bool hasDelivery,
		string? city,
		string? street,
		string? postalCode,
		List<DishDto>? dishes = null)
	{
		Id = id;
		Name = name;
		Description = description;
		Category = category;
		HasDelivery = hasDelivery;
		City = city;
		Street = street;
		PostalCode = postalCode;
		Dishes = dishes;
	}

	public int Id { get; init; }

	public string Name { get; init; } = null!;

	public string? Description { get; init; }

	public string? Category { get; init; }

	public bool HasDelivery { get; init; }

	public string? City { get; init; }

	public string? Street { get; init; }

	public string? PostalCode { get; init; }

	public List<DishDto>? Dishes { get; init; }

	public static RestaurantDto? FromEntity(Restaurant? entity)
	{
		if (entity is null)
			return null;

		return new(entity)
		{
			Dishes = DishDto.FromEntity(entity.Dishes)
		};
	}

	public static List<RestaurantDto> FromEntity(List<Restaurant> entities) => entities.Select(FromEntity).ToList()!;
}
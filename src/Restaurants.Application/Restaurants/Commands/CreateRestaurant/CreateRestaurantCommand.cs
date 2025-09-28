using Mediator;
using Restaurants.Domain.Common.Results;
using Restaurants.Domain.Entities.Restaurants;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public record CreateRestaurantCommand : IRequest<Result<int>>
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;

	public string Description { get; init; } = null!;

	public string Category { get; init; } = null!;

	public bool HasDelivery { get; init; }

	public string? ContactEmail { get; init; }

	public string? ContactNumber { get; init; }

	public string? City { get; init; }

	public string? Street { get; init; }

	public string? PostalCode { get; init; }

	public static Restaurant ToEntity(CreateRestaurantCommand command) => new()
	{
		Id = command.Id,
		Name = command.Name,
		Description = command.Description,
		Category = command.Category,
		HasDelivery = command.HasDelivery,
		ContactEmail = command.ContactEmail,
		ContactNumber = command.ContactNumber,
		Address = new()
		{
			City = command.City,
			Street = command.Street,
			PostalCode = command.PostalCode
		}
	};
}
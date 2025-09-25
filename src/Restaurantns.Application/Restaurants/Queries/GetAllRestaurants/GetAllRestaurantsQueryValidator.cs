using FluentValidation;
using Restaurantns.Domain.Entities;

namespace Restaurantns.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
	public GetAllRestaurantsQueryValidator()
	{

		var allowedSortNames = typeof(Restaurant).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Select(x => x.Name);

		RuleFor(x => x.PageNumber)
		.GreaterThanOrEqualTo(1);

		RuleFor(x => x.PageSize)
		.InclusiveBetween(1, 100);

		RuleFor(x => x.SortBy)
		.Must(x => allowedSortNames.Contains(x, StringComparer.OrdinalIgnoreCase))
		.When(x => x.SortBy is not null)
		.WithMessage($"Value should be on of [{string.Join(" , ", allowedSortNames)}] ");
	}
}
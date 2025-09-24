using FluentValidation;

namespace Restaurantns.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
	public GetAllRestaurantsQueryValidator()
	{
		RuleFor(x => x.PageNumber)
		.GreaterThanOrEqualTo(1);

		RuleFor(x => x.PageSize)
		.InclusiveBetween(1, 100);
	}
}
using FluentValidation;
using Restaurantns.Application.Restaurants.Commands.CreateRestaurant;

namespace Restaurantns.Application.Restaurants.Validators;

public class CreatRestaurantValidator : AbstractValidator<CreateRestaurantCommand>
{
	public CreatRestaurantValidator()
	{
		RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
		RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
		RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
		RuleFor(x => x.ContactEmail).EmailAddress().WithMessage("Please enter a valid e-mail address");

		RuleFor(x => x.ContactNumber)
			.Length(11)
			.WithMessage("Please enter a valid phone number");
	}
}
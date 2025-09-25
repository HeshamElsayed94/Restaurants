using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantValidator : AbstractValidator<CreateRestaurantCommand>
{
	public CreateRestaurantValidator()
	{
		RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
		RuleFor(x => x.Name)
		.Length(3, 100)
		.NotEmpty().WithMessage("Name is required");

		RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");

		RuleFor(x => x.ContactEmail).EmailAddress().WithMessage("Please enter a valid e-mail address");

		RuleFor(x => x.PostalCode)
		.Matches(@"^\d{2}-\d{3}$")
		.WithMessage("Please provide a valid postal code (XX-XXX)");

		RuleFor(x => x.ContactNumber)
			.Length(11)
			.WithMessage("Please enter a valid phone number");
	}
}
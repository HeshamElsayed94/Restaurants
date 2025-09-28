using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
{
	private readonly IHttpContextAccessor _contextAccessor;

	public CreateDishCommandValidator(IHttpContextAccessor contextAccessor)
	{
		_contextAccessor = contextAccessor;

		RuleFor(x => x.RestaurantId)
		.Must(x => x.ToString()
		.Equals(_contextAccessor.HttpContext!.Request.RouteValues.GetValueOrDefault("restaurantId")))
		.WithMessage($"{nameof(CreateDishCommand.RestaurantId)} must match restaurantId from route");

		RuleFor(x => x.Name)
		.Length(3, 100);

		RuleFor(x => x.Description)
		.Length(3, 1000);

		RuleFor(x => x.Price)
		.GreaterThanOrEqualTo(0.1m);

		RuleFor(x => x.KiloCalories)
		.GreaterThanOrEqualTo(0);
	}
}
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantValidator : AbstractValidator<UpdateRestaurantCommand>
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public UpdateRestaurantValidator(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;

		RuleFor(x => x.Id)
		.Must(x => x.ToString().Equals(_httpContextAccessor.HttpContext!
			.Request.RouteValues.SingleOrDefault(k => k.Key == "id").Value))
		.WithMessage("Id must match route value");

		RuleFor(x => x.Name)
			.Length(3, 100);

		RuleFor(x => x.Description)
			.Length(1, 1000);

	}
}
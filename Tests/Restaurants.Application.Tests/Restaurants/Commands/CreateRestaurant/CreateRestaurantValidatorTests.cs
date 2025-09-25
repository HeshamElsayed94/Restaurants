using FluentValidation.TestHelper;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantValidatorTests
{
	[Fact()]
	public void Validator_ForValidCommand_ShouldNotHaveAnyValidationErrors()
	{

		//arrange
		var command = new CreateRestaurantCommand()
		{
			Name = "Test",
			Category = "Italian",
			ContactEmail = "test@test.com",
			Description = "Test",
			PostalCode = "12-345",
		};

		var validator = new CreateRestaurantValidator();

		// act
		var result = validator.TestValidate(command);

		// assert
		result.ShouldNotHaveAnyValidationErrors();

	}

	[Fact()]
	public void Validator_ForValidCommand_ShouldHaveValidationErrors()
	{

		//arrange
		var command = new CreateRestaurantCommand()
		{
			Name = "T",
			Category = "Italian",
			ContactEmail = "@test.com",
			Description = "",
			PostalCode = "12-3456",
		};

		var validator = new CreateRestaurantValidator();

		// act
		var result = validator.TestValidate(command);

		// assert
		result.ShouldHaveValidationErrorFor(x => x.Name);
		result.ShouldHaveValidationErrorFor(x => x.ContactEmail);
		result.ShouldHaveValidationErrorFor(x => x.PostalCode);
		result.ShouldHaveValidationErrorFor(x => x.Description);

	}

	[Theory]
	[InlineData("10223")]
	[InlineData("10-23")]
	[InlineData("10 23")]
	[InlineData("10 323")]
	[InlineData("1-0-323")]
	public void Validator_ForInvalidPostalCode_ShouldHaveValidationErrorsForPostalCode(string postalCode)
	{

		// arrange
		var command = new CreateRestaurantCommand()
		{
			PostalCode = postalCode,
		};

		var validator = new CreateRestaurantValidator();

		// act
		var result = validator.TestValidate(command);

		// assert

		result.ShouldHaveValidationErrorFor(x => x.PostalCode);

	}
}
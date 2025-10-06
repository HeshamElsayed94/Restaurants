using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace Restaurants.Application.Behaviors;

public class CustomValidationResultFactory : IFluentValidationAutoValidationResultFactory
{

	public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
	{

		if (validationProblemDetails!.Errors.ContainsKey("$"))
		{
			validationProblemDetails!.Status = StatusCodes.Status400BadRequest;
			validationProblemDetails.Title = "One or more errors occurred.";
			return new BadRequestObjectResult(validationProblemDetails);
		}

		validationProblemDetails!.Status = StatusCodes.Status422UnprocessableEntity;
		validationProblemDetails.Type = "https://tools.ietf.org/html/rfc4918#section-11.2";
		return new UnprocessableEntityObjectResult(validationProblemDetails);

	}
}
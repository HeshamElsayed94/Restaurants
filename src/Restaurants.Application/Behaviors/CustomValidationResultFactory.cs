using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace Restaurants.Application.Behaviors;

public class CustomValidationResultFactory : IFluentValidationAutoValidationResultFactory
{

	public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
	{

		validationProblemDetails!.Status = StatusCodes.Status422UnprocessableEntity;

		if (validationProblemDetails.Errors.ContainsKey("$"))
			return new BadRequestObjectResult(validationProblemDetails);

		return new UnprocessableEntityObjectResult(validationProblemDetails);

	}
}
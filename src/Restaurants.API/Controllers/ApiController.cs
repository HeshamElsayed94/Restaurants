using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Domain.Common.Results;

namespace Restaurants.API.Controllers;
[ApiController]
public class ApiController : ControllerBase
{
	protected ActionResult Problem(List<Error> errors)
	{
		if (errors.Count is 0)
			return Problem();

		if (errors.All(err => err.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity))
			return ValidationProblem(errors);

		return Problem(errors[0]);
	}

	private ObjectResult Problem(Error error)
		=> Problem(statusCode: (int)error.StatusCode, title: error.Description);

	private ActionResult ValidationProblem(List<Error> errors)
	{
		errors.ForEach(err => ModelState.AddModelError(err.Code, err.Description));

		var problemDetails = new ValidationProblemDetails(ModelState)
		{
			Type = "https://tools.ietf.org/html/rfc4918#section-11.2",
			Instance = $"{HttpContext.Request.Method} {HttpContext.Request.Path}",
		};
		problemDetails.Extensions.Add("requestId", HttpContext.TraceIdentifier);
		problemDetails.Extensions.Add("traceId", Activity.Current?.Id);

		return new UnprocessableEntityObjectResult(problemDetails);
	}
}
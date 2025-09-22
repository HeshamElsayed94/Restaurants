using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;

namespace Restaurantns.API.Exceptions;

public class ExceptionHandler(
	ILogger<ExceptionHandler> logger,
	IProblemDetailsService problemDetails,
	IHostEnvironment env) : IExceptionHandler
{

	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{

		logger.LogError(exception, exception.Message);

		httpContext.Response.StatusCode = exception switch
		{
			ValidationException or FluentValidation.ValidationException => StatusCodes.Status422UnprocessableEntity,
			_ => StatusCodes.Status500InternalServerError
		};

		return await problemDetails.TryWriteAsync(new()
		{
			HttpContext = httpContext,
			Exception = exception,
			ProblemDetails = new()
			{
				Type = exception.GetType().Name,
				Title = "Error occured",
				Detail = env.IsDevelopment() ? exception.StackTrace : exception.Message
			}
		});

	}
}
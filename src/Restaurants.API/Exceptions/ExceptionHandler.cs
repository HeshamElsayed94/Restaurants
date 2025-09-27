using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;

namespace Restaurants.API.Exceptions;

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
			ValidationException => StatusCodes.Status400BadRequest,
			_ => StatusCodes.Status500InternalServerError
		};

		return await problemDetails.TryWriteAsync(new()
		{
			HttpContext = httpContext,
			Exception = exception,
			ProblemDetails = new()
			{
				Type = exception.GetType().Name,
				Title = "Error occurred",
				Detail = env.IsDevelopment() ? exception.StackTrace : exception.Message
			}
		});

	}
}
using System.Diagnostics;
using Mediator;
using Microsoft.Extensions.Logging;

namespace Restaurants.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
: IPipelineBehavior<TRequest, TResponse>
where TRequest : IMessage

{
	public async ValueTask<TResponse> Handle(TRequest request, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken ct)
	{
		logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
		typeof(TRequest).Name, typeof(TResponse).Name, request);

		var timer = new Stopwatch();
		timer.Start();

		var response = await next(request, ct);

		timer.Stop();
		var timeTaken = timer.Elapsed;

		if (timeTaken.Seconds > 3)
		{
			logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken}s",
			typeof(TRequest).Name, timeTaken.Seconds);
		}

		return response;
	}
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Restaurants.API.RequestHelpers;

namespace Restaurants.API.Filters;

public class HttpHeaderCacheFilter : IAsyncResultFilter
{
	public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
	{
		if (context.Result is OkObjectResult result && result.Value is not null)
		{

			string eTag = ETagGenerator.GenerateETag(result.Value);

			if (context.HttpContext.Request.Headers.IfNoneMatch == eTag)
			{
				context.Result = new StatusCodeResult(StatusCodes.Status304NotModified);
			}
			else
			{
				const int expiresInSeconds = 60;
				string expires = DateTime.UtcNow.AddSeconds(expiresInSeconds).ToString("R");
				context.HttpContext.Response.Headers.Expires = new(expires);
				context.HttpContext.Response.Headers.CacheControl = new StringValues($"public,max-age={expiresInSeconds},s-maxage=600,must-revalidate");
			}

			context.HttpContext.Response.Headers.ETag = eTag;

		}

		await next();
	}
}
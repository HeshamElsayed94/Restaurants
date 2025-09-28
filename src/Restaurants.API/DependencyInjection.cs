using System.Text.Json.Serialization;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using Restaurants.API.Exceptions;
using Serilog;

namespace Restaurants.API;

public static class DependencyInjection
{
	public static void AddPresentation(this WebApplicationBuilder builder)
	{
		builder.Services.AddExceptionHandler<ExceptionHandler>();

		builder.Services.AddAuthentication();

		builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

		builder.Services.AddSwaggerGen(c =>
			{
				c.AddSecurityDefinition("Bearer", new()
				{
					Type = SecuritySchemeType.Http,
					Scheme = "Bearer"
				});

				c.AddSecurityRequirement(new()
				{
					{
						new() { Reference = new() { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
						[]
					}
				});
			}
		);

		builder.Services.AddProblemDetails(options => options.CustomizeProblemDetails = context =>
		{
			context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
			context.ProblemDetails.Extensions.Add("requestId", context.HttpContext.TraceIdentifier);
		});

		builder.Services.AddControllers()
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
				options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			});

		builder.Services.Scan(scan =>
		{
			scan.FromAssembliesOf(typeof(Application.Extensions.DependencyInjection))
			.AddClasses()
			.AsMatchingInterface()
			.WithScopedLifetime();

			scan.FromAssembliesOf(typeof(Infrastructure.Extensions.DependencyInjection))
			.AddClasses(false)
			.AsMatchingInterface()
			.WithScopedLifetime();

		});

		builder.Services.AddEndpointsApiExplorer();

		builder.Services.AddResponseCompression(op =>
		{
			op.Providers.Add<GzipCompressionProvider>();
			op.Providers.Add<BrotliCompressionProvider>();
			op.MimeTypes = ["application/json", "application/xml", "text/plain", "text/html"];
		});

		builder.Services.AddHttpCacheHeaders(op =>
		{
			op.CacheLocation = CacheLocation.Public;
			op.SharedMaxAge = 600;
		}, vOp => vOp.MustRevalidate = true);

	}
}
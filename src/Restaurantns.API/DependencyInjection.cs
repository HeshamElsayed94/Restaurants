using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Restaurantns.API.Exceptions;
using Serilog;

namespace Restaurantns.API;

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

		builder.Services.Scan(options => options.FromAssembliesOf(typeof(Infrastructure.Extensions.DependencyInjection))
			.AddClasses(false)
			.AsImplementedInterfaces()
			.WithScopedLifetime());

		builder.Services.AddEndpointsApiExplorer();

	}
}
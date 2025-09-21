using System.Text.Json.Serialization;
using Restaurantns.API.Exceptions;
using Restaurantns.Application.Extensions;
using Restaurantns.Infrastructure.Extensions;
using Restaurantns.Infrastructure.Seeders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

// builder.Services.

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

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication();

builder.Services.Scan(options =>
	options.FromAssembliesOf(typeof(IRestaurantSeeder))
		.AddClasses(false)
		.AsImplementedInterfaces()
		.WithScopedLifetime()
);

var app = builder.Build();

app.UseExceptionHandler();

using var scope = app.Services.CreateScope();
await scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>().Seed();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
using System.Text.Json.Serialization;
using Restaurantns.Application.Contracts;
using Restaurantns.Application.Extensions;
using Restaurantns.Infrastructure.Extensions;
using Restaurantns.Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.IgnoreNullValues = true;
		options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication();

builder.Services.Scan(options =>
	options.FromAssembliesOf(typeof(IRestaurantSeeder), typeof(IRestaurantService))
		.AddClasses(false)
		.AsImplementedInterfaces()
		.WithScopedLifetime()
);

var app = builder.Build();

using var scope = app.Services.CreateScope();
await scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>().Seed();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
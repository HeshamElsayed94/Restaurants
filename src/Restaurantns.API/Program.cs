using Restaurantns.Infrastructure.Extensions;
using Restaurantns.Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.Scan(options =>
	options.FromAssemblyOf<IRestaurantSeeder>()
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
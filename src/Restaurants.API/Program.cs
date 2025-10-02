using Delta;
using Microsoft.Extensions.Caching.Hybrid;
using Restaurants.API;
using Restaurants.API.Middlewares;
using Restaurants.Application.Extensions;
using Restaurants.Application.Restaurants.Queries.Caching;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication();

builder.AddPresentation();

var app = builder.Build();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
	app.UseHsts();
}

using var scope = app.Services.CreateScope();
{
	await scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>().Seed();
	var cache = scope.ServiceProvider.GetRequiredService<HybridCache>();

	await cache.RemoveByTagAsync([RestaurantCachingTags.Main, "Users"]);
	var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
	logger.LogInformation("All cache removed");
}

app.UseSerilogRequestLogging();

app.UseResponseCompression();

app.UseAuthentication();

app.UseMiddleware<ValidateTokenSecurityStampMiddleware>();

app.UseAuthorization();

app.UseDelta();

app.MapGroup("api/identity")
	.WithTags("Identity")
	.MapIdentityApi<User>();

app.MapControllers();

app.Run();
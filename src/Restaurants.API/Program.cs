using Restaurants.API;
using Restaurants.API.Middlewares;
using Restaurants.Application.Contracts;
using Restaurants.Application.Extensions;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.AddPresentation();

builder.Services.AddApplication();

var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
else
{
	app.UseHsts();
}

app.UseExceptionHandler();

using var scope = app.Services.CreateScope();
await scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>().Seed();

app.UseSerilogRequestLogging();

app.UseAuthentication();

app.UseMiddleware<ValidateTokenSecurityStampMiddleware>();

app.UseAuthorization();

app.MapGroup("api/identity")
	.WithTags("Identity")
	.MapIdentityApi<User>();

app.MapControllers();

app.Run();

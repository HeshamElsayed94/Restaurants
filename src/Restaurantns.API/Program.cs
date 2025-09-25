using Restaurantns.API;
using Restaurantns.API.Middlewares;
using Restaurantns.Application.Contracts;
using Restaurantns.Application.Extensions;
using Restaurantns.Domain.Entities;
using Restaurantns.Infrastructure.Extensions;
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
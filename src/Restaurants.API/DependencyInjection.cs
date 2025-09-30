using System.Text.Json.Serialization;
using Mediator;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using Restaurants.API.Exceptions;
using Restaurants.API.Filters;
using Restaurants.Application.Common;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Query.Caching;
using Restaurants.Application.Dishes.Query.GetByIdForRestaurant;
using Restaurants.Application.Dishes.Query.GetDishesForRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.Caching;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Domain.Common.Results;
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

		builder.Services.AddControllers(op => op.Filters.Add<HttpHeaderCacheFilter>())
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
			.AddClasses(false).AsMatchingInterface().WithScopedLifetime();

		});

		builder.Services.AddEndpointsApiExplorer();

		builder.Services.AddResponseCompression(op =>
		{
			op.Providers.Add<GzipCompressionProvider>();
			op.Providers.Add<BrotliCompressionProvider>();
			op.MimeTypes = ["application/json", "application/xml", "text/plain", "text/html"];
		});

		builder.Services.AddHybridCache(op => op.DefaultEntryOptions = new()
		{
			Expiration = TimeSpan.FromMinutes(30),
			LocalCacheExpiration = TimeSpan.FromMinutes(10),
		});

		builder.Services.AddScoped<IRequestHandler<GetAllRestaurantsQuery, PagedList<RestaurantDto>>, GetAllRestaurantsQueryHandler>();
		builder.Services.Decorate<IRequestHandler<GetAllRestaurantsQuery, PagedList<RestaurantDto>>, CachedGetAllRestaurantsQueryHandler>();

		builder.Services.AddScoped<IRequestHandler<GetRestaurantByIdQuery, Result<RestaurantDto>>, GetRestaurantByIdQueryHandler>();
		builder.Services.Decorate<IRequestHandler<GetRestaurantByIdQuery, Result<RestaurantDto>>, CachedGetRestaurantByIdQueryHandler>();

		builder.Services.AddScoped<IRequestHandler<GetDishByIdForRestaurantQuery, Result<DishDto>>, GetDishByIdForRestaurantQueryHandler>();
		builder.Services.Decorate<IRequestHandler<GetDishByIdForRestaurantQuery, Result<DishDto>>, CachedGetDishByIdForRestaurantQueryHandler>();

		builder.Services.AddScoped<IRequestHandler<GetDishesForRestaurantQuery, Result<IEnumerable<DishDto>>>, GetDishesForRestaurantQueryHandler>();
		builder.Services.Decorate<IRequestHandler<GetDishesForRestaurantQuery, Result<IEnumerable<DishDto>>>, CachedGetDishesForRestaurantQueryHandler>();
	}
}
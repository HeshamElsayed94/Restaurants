using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Behaviors;
using Restaurants.Application.Common;
using Restaurants.Application.Common.Results;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Query.Caching;
using Restaurants.Application.Dishes.Query.GetDishByIdForRestaurant;
using Restaurants.Application.Dishes.Query.GetDishesForRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.Caching;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Restaurants.Application.Extensions;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{

		services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

		services.AddFluentValidationAutoValidation(configuration
			=> configuration.OverrideDefaultResultFactoryWith<CustomValidationResultFactory>());

		services.AddMediator(optionts => optionts.ServiceLifetime = ServiceLifetime.Scoped);

		services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
			.AddClasses()
			.AsMatchingInterface()
			.WithScopedLifetime());

		services.AddScoped<IRequestHandler<GetAllRestaurantsQuery, PagedList<RestaurantDto>>, GetAllRestaurantsQueryHandler>();
		services.Decorate<IRequestHandler<GetAllRestaurantsQuery, PagedList<RestaurantDto>>, CachedGetAllRestaurantsQueryHandler>();

		services.AddScoped<IRequestHandler<GetRestaurantByIdQuery, Result<RestaurantDto>>, GetRestaurantByIdQueryHandler>();
		services.Decorate<IRequestHandler<GetRestaurantByIdQuery, Result<RestaurantDto>>, CachedGetRestaurantByIdQueryHandler>();

		services.AddScoped<IRequestHandler<GetDishByIdForRestaurantQuery, Result<DishDto>>, GetDishByIdForRestaurantQueryHandler>();
		services.Decorate<IRequestHandler<GetDishByIdForRestaurantQuery, Result<DishDto>>, CachedGetDishByIdForRestaurantQueryHandler>();

		services.AddScoped<IRequestHandler<GetDishesForRestaurantQuery, Result<IEnumerable<DishDto>>>, GetDishesForRestaurantQueryHandler>();
		services.Decorate<IRequestHandler<GetDishesForRestaurantQuery, Result<IEnumerable<DishDto>>>, CachedGetDishesForRestaurantQueryHandler>();

		return services;
	}
}
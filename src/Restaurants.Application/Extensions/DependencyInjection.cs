using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Behaviors;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Restaurants.Application.Extensions;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{

		var assembly = typeof(DependencyInjection).Assembly;

		services.AddValidatorsFromAssembly(assembly);

		services.AddFluentValidationAutoValidation(configuration
			=> configuration.OverrideDefaultResultFactoryWith<CustomValidationResultFactory>());

		services.AddMediatR(cofig => cofig.RegisterServicesFromAssembly(assembly));

		services.AddHttpContextAccessor();

		return services;
	}
}
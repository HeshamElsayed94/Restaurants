using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Restaurantns.Application.Extensions;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{

		var assembly = typeof(DependencyInjection).Assembly;

		services.AddValidatorsFromAssembly(assembly);
		services.AddFluentValidationAutoValidation();

		services.AddMediatR(cofig => cofig.RegisterServicesFromAssembly(assembly));

		return services;
	}
}
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ITI_MVC_Project.Core
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddCoreDependencyInjection(this IServiceCollection services, IConfiguration configuration)
		{
			//services.AddMapsterConfig();
			services.AddFluentValidationConfig();
			return services;
		}
		//private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
		//{
		//	var mappingConfig = TypeAdapterConfig.GlobalSettings;
		//	mappingConfig.Scan(Assembly.GetExecutingAssembly());

		//	services.AddSingleton<IMapper>(new Mapper(mappingConfig));

		//	return services;
		//}
		private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
		{
			services
				.AddFluentValidationAutoValidation()
				.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

			return services;
		}
	}
}

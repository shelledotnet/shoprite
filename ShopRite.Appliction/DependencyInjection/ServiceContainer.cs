using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopRite.Application.Mapping;
using ShopRite.Application.Services;
using ShopRite.Application.Services.Implementation;
using ShopRite.Application.Services.Interfaces;
using ShopRite.Domain.Entities;
using ShopRite.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            var projectOptions = config.GetSection(nameof(ProjectOptions)).Get<ProjectOptions>();

            services.AddScoped<IProductService ,ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();


            //it will register Authomapper service and  also scan this Assembly/Project in this class MappingConfig for any type that inherit AutoMapper Profile
            services.AddAutoMapper(typeof(MappingConfig).Assembly);


            services.AddOptions<ProjectOptions>()
                  .BindConfiguration(nameof(ProjectOptions))
                  .ValidateDataAnnotations()
                   .Validate(options =>
                   {
                       if (options.XApiKey != projectOptions?.XApiKeyMap) return false;
                       return true;
                   })
                 .ValidateOnStart();




            return services;
        }
    }
}

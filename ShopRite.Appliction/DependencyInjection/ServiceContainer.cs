using Microsoft.Extensions.DependencyInjection;
using ShopRite.Application.Mapping;
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
        public static IServiceCollection AddInfraustureService(this IServiceCollection services)
        {

            services.AddScoped<IProductService ,ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();


            //it will register Authomapper service and  also scan this Assembly/Project in this class MappingConfig for any type that inherit AutoMapper Profile
            services.AddAutoMapper(typeof(MappingConfig).Assembly);
            return services;
        }
    }
}

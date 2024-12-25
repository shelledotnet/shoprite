using AutoMapper;
using ShopRite.Application.Dto;
using ShopRite.Application.Dto.Category;
using ShopRite.Application.Dto.Identity;
using ShopRite.Application.Dto.Product;
using ShopRite.Domain.Entities;
using ShopRite.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Application.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            #region Mapping Left to right

            #endregion

            CreateMap<CreateCategory, Category>();

             CreateMap<CreateProduct, Product>();

            CreateMap<Category, GetCategory>();
            CreateMap<Product, GetProduct>();
            CreateMap<UpdateProduct, Product>();
            CreateMap<ServiceResponse<GetCategory>, ServiceFailedResponse>();
            CreateMap<ServiceResponse<string>, ServiceFailedResponse>();
            CreateMap<ServiceResponse<IEnumerable<GetCategory>>, ServiceFailedResponse>();
            CreateMap<ServiceResponse<GetProduct>, ServiceFailedResponse>();
            CreateMap<ServiceResponse<IEnumerable<GetProduct>>, ServiceFailedResponse>();
            CreateMap<CreateUser, ApplicationUser>();
            CreateMap<LoginUser, ApplicationUser>();

            //ServiceResponse<IEnumerable<GetProduct>>  
        }
    }
}

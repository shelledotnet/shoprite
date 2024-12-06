using AutoMapper;
using ShopRite.Application.Dto.Category;
using ShopRite.Application.Dto.Product;
using ShopRite.Domain.Entities;
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

        }
    }
}

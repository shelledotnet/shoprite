using AutoMapper;
using ShopRite.Application.Dto;
using ShopRite.Application.Dto.Product;
using ShopRite.Application.Services.Interfaces;
using ShopRite.Domain.Entities;
using ShopRite.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Application.Services.Implementation
{
    //usin primary constructor
    public class ProductService(IGeneric<Product> productInterface, IMapper mapper) : IProductService
    {

        public async Task<ServiceResponse<string>> AddAsync(CreateProduct createProduct)
        {
            try
            {
                Product product = mapper.Map<Product>(createProduct);
                int result = await productInterface.AddAsync(product);
                return result > 0 ?
                     new ServiceResponse<string>("product created successfully",
                          HttpStatusCode.Created, true, "successful") :
                 new ServiceResponse<string>("product creation fail",
                        HttpStatusCode.UnprocessableEntity, false, "failed");
            }
            catch (Exception ex)
            {
                //log this
                return new ServiceResponse<string>("error when creating product",
                          HttpStatusCode.UnprocessableEntity, false, "failed");
            }
        }

        public async Task<ServiceResponse<string>> DeleteAsync(Guid id)
        {
            try
            {
                int result = await productInterface.DeleteAsync(id);
             return  result > 0 ?
                     new ServiceResponse<string>("product deleted successfully",
                          HttpStatusCode.NoContent, true, "successful") :
                 new ServiceResponse<string>("product  failed to deleted",
                        HttpStatusCode.UnprocessableEntity, false, "failed");
            }
            catch (Exception ex)
            {
                //log this
                return new ServiceResponse<string>("error when deleting product",
                          HttpStatusCode.UnprocessableEntity, false, "failed");
            }
        }

        public async Task<ServiceResponse<IEnumerable<GetProduct>>> GetAllAsync()
        {
            try
            {
                var products = await productInterface.GetAllAsync();
                return products != null ?
                      new ServiceResponse<IEnumerable<GetProduct>>(mapper.Map<IEnumerable<GetProduct>>(products), HttpStatusCode.OK, true, "success") :
                      new ServiceResponse<IEnumerable<GetProduct>>(null, HttpStatusCode.NotFound, false, "product not found");

            }
            catch (Exception)
            {
                //log this
                return new ServiceResponse<IEnumerable<GetProduct>>(null,
                       HttpStatusCode.UnprocessableEntity, false, "failed");

            }
        }

        public async Task<ServiceResponse<GetProduct>> GetByIdAsync(Guid id)
        {
            try
            {
                Product product = await productInterface.GetByIdAsync(id);
                return product != null ?
                    new ServiceResponse<GetProduct>(mapper.Map<GetProduct>(product), HttpStatusCode.OK, true, "success") :
                    new ServiceResponse<GetProduct>(null, HttpStatusCode.UnprocessableEntity, false, "failed");
            }
            catch (Exception)
            {
                //log the exception
                return new ServiceResponse<GetProduct>(null, HttpStatusCode.InternalServerError, false, "failed");

            }
        }

        public async Task<ServiceResponse<string>> UpdateAsync(UpdateProduct updateProduct)
        {
            try
            {
                int update = await productInterface.UpdateAsync(mapper.Map<Product>(updateProduct));
                return update > 0 ?
                    new ServiceResponse<string>("update was successful", HttpStatusCode.NoContent, true, "success") :
                    new ServiceResponse<string>("update wasn't successful", HttpStatusCode.UnprocessableEntity, false, "failed");

            }
            catch (Exception)
            {
                //log this 
                return new ServiceResponse<string>("error when updating product", HttpStatusCode.UnprocessableEntity, false, "failed");

            }
        }
    }
}

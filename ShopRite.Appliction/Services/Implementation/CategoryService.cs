using AutoMapper;
using ShopRite.Application.Dto;
using ShopRite.Application.Dto.Category;
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
    //services are client phasing
    //using primary constructor
    public class CategoryService(IGeneric<Category> categoryInterface, IMapper mapper) : ICategoryService
    {
        public async Task<ServiceResponse<string>> AddAsync(CreateCategory createCategory)
        {
            try
            {
                Category category = mapper.Map<Category>(createCategory);
                int result = await categoryInterface.AddAsync(category);
                return result > 0 ?
                     new ServiceResponse<string>("category created successfully",
                          HttpStatusCode.Created, true, "successful") :
                 new ServiceResponse<string>("category creation fail",
                        HttpStatusCode.UnprocessableEntity, false, "failed");
            }
            catch (Exception ex)
            {
                //log this
                return new ServiceResponse<string>("error when creating category",
                          HttpStatusCode.UnprocessableEntity, false, "failed");
            }
        }

        public async Task<ServiceResponse<string>> DeleteAsync(Guid id)
        {
            try
            {
                int result = await categoryInterface.DeleteAsync(id);
                return result > 0 ?
                        new ServiceResponse<string>("category deleted successfully",
                             HttpStatusCode.NoContent, true, "successful") :
                    new ServiceResponse<string>("category  failed to be deleted",
                           HttpStatusCode.UnprocessableEntity, false, "failed");
            }
            catch (Exception ex)
            {
                //log this
                return new ServiceResponse<string>("error when deleting category",
                          HttpStatusCode.UnprocessableEntity, false, "failed");
            }
        }

        public async Task<ServiceResponse<IEnumerable<GetCategory>>> GetAllAsync()
        {
            try
            {
                var category = await categoryInterface.GetAllAsync(); //IEnumerabe is nullable prefable to use count to check if a record exist
                return category.Count() > 0 ?
                      new ServiceResponse<IEnumerable<GetCategory>>(mapper.Map<IEnumerable<GetCategory>>(category), HttpStatusCode.OK, true, "success") :
                      new ServiceResponse<IEnumerable<GetCategory>>(null, HttpStatusCode.NotFound, false, "category not found");

            }
            catch (Exception)
            {
                //log this
                return new ServiceResponse<IEnumerable<GetCategory>>(null,
                       HttpStatusCode.UnprocessableEntity, false, "failed");

            }
        }

        public async Task<ServiceResponse<GetCategory>> GetByIdAsync(Guid id)
        {
            try
            {
                Category category = await categoryInterface.GetByIdAsync(id);
                return category != null ?
                    new ServiceResponse<GetCategory>(mapper.Map<GetCategory>(category), HttpStatusCode.OK, true, "success") :
                    new ServiceResponse<GetCategory>(null, HttpStatusCode.UnprocessableEntity, false, "failed");
            }
            catch (Exception)
            {
                //log the exception
                return new ServiceResponse<GetCategory>(null, HttpStatusCode.InternalServerError, false, "failed");

            }
        }

        public async Task<ServiceResponse<string>> UpdateAsync(UpdateCategory updateCategory)
        {
            try
            {
                int update = await categoryInterface.UpdateAsync(mapper.Map<Category>(updateCategory));
                return update > 0 ?
                    new ServiceResponse<string>("category updated", HttpStatusCode.NoContent, true, "success") :
                    new ServiceResponse<string>("category failed to be updated", HttpStatusCode.UnprocessableEntity, false, "failed");

            }
            catch (Exception)
            {
                //log this 
                return new ServiceResponse<string>("error when updating category", HttpStatusCode.UnprocessableEntity, false, "failed");

            }
        }
    }
}

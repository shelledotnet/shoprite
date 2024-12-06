using ShopRite.Application.Dto;
using ShopRite.Application.Dto.Category;
using ShopRite.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Application.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        public Task<ServiceResponse<GetCategory>> AddAsync(CreateCategory createCategory)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<dynamic>> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<IEnumerable<GetCategory>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<GetCategory>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<GetCategory>> UpdateAsync(UpdateCategory updateCategory)
        {
            throw new NotImplementedException();
        }
    }
}

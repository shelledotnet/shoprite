using ShopRite.Application.Dto;
using ShopRite.Application.Dto.Category;
using ShopRite.Application.Dto.Product;

namespace ShopRite.Application.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ServiceResponse<GetCategory>> GetByIdAsync(Guid id);
        Task<ServiceResponse<IEnumerable<GetCategory>>> GetAllAsync();
        Task<ServiceResponse<GetCategory?>> AddAsync(CreateCategory createCategory);
        Task<ServiceResponse<string>> DeleteAsync(Guid id);
        Task<ServiceResponse<string>> UpdateAsync(UpdateCategory updateCategory);
    }
}

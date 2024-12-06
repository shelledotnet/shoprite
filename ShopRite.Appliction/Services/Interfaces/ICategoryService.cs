using ShopRite.Application.Dto;
using ShopRite.Application.Dto.Category;

namespace ShopRite.Application.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ServiceResponse<GetCategory>> GetByIdAsync(Guid id);
        Task<ServiceResponse<IEnumerable<GetCategory>>> GetAllAsync();
        Task<ServiceResponse<GetCategory>> AddAsync(CreateCategory createCategory);
        Task<ServiceResponse<dynamic>> DeleteAsync(Guid id);
        Task<ServiceResponse<GetCategory>> UpdateAsync(UpdateCategory updateCategory);
    }
}

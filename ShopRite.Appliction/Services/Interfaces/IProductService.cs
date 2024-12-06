using ShopRite.Application.Dto;
using ShopRite.Application.Dto.Product;

namespace ShopRite.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResponse<GetProduct>> GetByIdAsync(Guid id);
        Task<ServiceResponse<IEnumerable<GetProduct>>> GetAllAsync();
        Task<ServiceResponse<string>> AddAsync(CreateProduct createProduct);
        Task<ServiceResponse<string>> DeleteAsync(Guid id);
        Task<ServiceResponse<string>> UpdateAsync(UpdateProduct updateProduct);
    }
}

using ShopRite.Domain.Entities.Identity;

namespace ShopRite.Domain.Interface.Authentication
{
    public interface IRoleManagement
    {
        Task<IEnumerable<string>> GetUserRoleAsync(string userEmail);
        Task<bool> AddUserToRoleAsync(ApplicationUser applicationUser,string roleName);
    }
}

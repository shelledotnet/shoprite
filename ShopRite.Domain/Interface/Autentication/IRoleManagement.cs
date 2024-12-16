using ShopRite.Domain.Entities.Identity;

namespace ShopRite.Domain.Interface.Autentication
{
    public interface IRoleManagement
    {
        Task<string> GetUserRoleAsync(string userEmail);
        Task<bool> AddUserToRoleAsync(ApplicationUser applicationUser,string roleName);
    }
}

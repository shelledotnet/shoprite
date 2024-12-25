using ShopRite.Domain.Entities.Identity;
using System.Security.Claims;

namespace ShopRite.Domain.Interface.Authentication
{
    public interface IUserManagement
    {
        Task<bool> CreateUserAsync(ApplicationUser applicationUser);
        Task<bool> LoginUserAsync(ApplicationUser applicationUser);

        Task<ApplicationUser?> GetUserByEmailAsync(string email);  //this is nullable
        Task<ApplicationUser?> GetUserByIdAsync(string id);  //this is not nullable 

        Task<IEnumerable<ApplicationUser>?> GetAllUsersAsync();//this can return null

        Task<int> RemoveUserByEmailAsync(string email);          

        Task<ClaimsIdentity> GetUserClaimAsync(string email);  
    }
}

using ShopRite.Domain.Entities.Identity;
using System.Security.Claims;

namespace ShopRite.Domain.Interface.Autentication
{
    public interface IUserManagement
    {
        Task<bool> CreateUserAsync(ApplicationUser applicationUser);
        Task<bool> LoinUserAsync(ApplicationUser applicationUser);

        Task<ApplicationUser?> GetUserByEmailAsync(string email);  //this is nullable
        Task<ApplicationUser> GetUserByIdAsync(string id);  //this is not nullable 

        Task<IEnumerable<ApplicationUser>?> GetAllUsersAsync();//this can return null

        Task<int> RemoveUserByEmailAsync(string email);          

        Task<List<Claim>> GetUserClaimAsync(string email);  
    }
}

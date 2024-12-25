using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopRite.Domain.Entities.Identity;
using ShopRite.Domain.Interface.Authentication;
using ShopRite.Infrastructure.Data;
using System.Security.Claims;

namespace ShopRite.Infrastructure.Repository.Authentiction
{
    public class UserManagement
        (IRoleManagement roleManagement,UserManager<ApplicationUser> userManager, AppDbContext context) : IUserManagement
    {
        public async Task<bool> CreateUserAsync(ApplicationUser applicationUser)
        {
            var user = await GetUserByEmailAsync(applicationUser.Email!);
            if (user != null) return false;

            return (await userManager.CreateAsync(applicationUser, applicationUser?.PasswordHash!)).Succeeded;  
            
        }

        public async Task<IEnumerable<ApplicationUser>?> GetAllUsersAsync()
              => await context.Users.ToListAsync();
        

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
            => await userManager.FindByEmailAsync(email);
        

        public async Task<ApplicationUser?> GetUserByIdAsync(string id)
            => await userManager.FindByIdAsync(id);
        

        public async Task<ClaimsIdentity> GetUserClaimAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);
            var roles = await roleManagement.GetUserRoleAsync(user!.Email!);

            ClaimsIdentity? claims =new( [
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
               new Claim("FullName",user.FullName),
               new Claim("IsBlocked", user.Blocked.ToString()),
                new Claim("IsActive", user.Active.ToString()),
               new Claim(ClaimTypes.Email,user.Email ?? ""),
               new Claim("jti",Guid.NewGuid().ToString()), //to identify the refereshtoken id
               new Claim("UserName",user.Email ?? "")
                ]);
            foreach (var role in roles)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        public async Task<bool> LoginUserAsync(ApplicationUser applicationUser)
        {
          var user = await GetUserByEmailAsync (applicationUser.Email!);
          if(user == null) return false;

          var roles = await roleManagement.GetUserRoleAsync(user!.Email!);
          if(roles.Count() < 1) return false;

          return (await userManager.CheckPasswordAsync(user,applicationUser.PasswordHash!));
        }

        public async Task<int> RemoveUserByEmailAsync(string email)
        {
            var users = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if(users == null) return 0;
            context.Users.Remove(users);
            return await context.SaveChangesAsync();
        }
    }
}

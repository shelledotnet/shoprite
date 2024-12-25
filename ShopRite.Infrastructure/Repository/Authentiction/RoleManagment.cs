using Microsoft.AspNetCore.Identity;
using ShopRite.Domain.Entities.Identity;
using ShopRite.Domain.Interface.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Infrastructure.Repository.Authentiction
{
    //use of primary constructor
    public class RoleManagment(UserManager<ApplicationUser> userManager) : IRoleManagement
    {
        public async Task<bool> AddUserToRoleAsync(ApplicationUser applicationUser, string roleName)=>
           (await userManager.AddToRoleAsync(applicationUser, roleName)).Succeeded;
        

        public async Task<IEnumerable<string>> GetUserRoleAsync(string userEmail)
        {
            var user  =  await userManager.FindByEmailAsync(userEmail);
            return  await userManager.GetRolesAsync(user!);        
        }
    }
}

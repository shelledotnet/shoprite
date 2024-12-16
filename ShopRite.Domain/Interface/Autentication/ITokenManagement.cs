using System.Security.Claims;

namespace ShopRite.Domain.Interface.Autentication
{
    public interface ITokenManagement
    {
        Task<string> GetRefresTokenAsync();

        Task<List<Claim>> GetUserClaimsFromTokenAsync(string email);

        Task<bool> ValidateReFresTokenAsync(string refrestoken);

        Task<string> GetUserIdByReFresTokenAsync(string refrestoken);

        Task<int> AddReFresTokenAsync(string userId,string refrestoken);

        Task<int> UpdateReFresTokenAsync(string userId, string refrestoken);

        Task<string> GenerateTokenAsync(List<Claim> claims);
    }
}

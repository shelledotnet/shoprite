using Microsoft.IdentityModel.Tokens;
using ShopRite.Application.Dto;
using System.Security.Claims;

namespace ShopRite.Domain.Interface.Authentication
{
    public interface ITokenManagement
    {
        String GenerateRefresToken();

        List<Claim> GetUserClaimsFromTokenAsync(string token);

        Task<bool> ValidateReFresTokenAsync(string refrestoken);

        Task<string> GetUserIdByReFresTokenAsync(string refrestoken);

        Task<int> AddReFresTokenAsync(string userId,string refrestoken, SecurityToken token);

        Task<int> UpdateReFresTokenAsync(string userId, string refrestoken,string jwtId);

        Task RemovedUnusedRefeshTokenAsync(string userId);

        TokenRequirement GenerateToken(ClaimsIdentity claimsIdentity);
    }
}

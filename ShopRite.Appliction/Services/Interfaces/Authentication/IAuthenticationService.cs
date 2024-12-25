using ShopRite.Application.Dto;
using ShopRite.Application.Dto.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Application.Services.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task<ServiceResponse<string>> CreateUser(CreateUser createUser);
        Task<ServiceResponse<LoginResponse>> Login(LoginUser loginUser);

        Task<ServiceResponse<LoginResponse>> ReviveToken(string refreshToken);

    }
}

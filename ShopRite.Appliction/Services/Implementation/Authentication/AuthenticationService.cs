using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using ShopRite.Application.Dto;
using ShopRite.Application.Dto.Identity;
using ShopRite.Application.Services.Interfaces.Authentication;
using ShopRite.Application.Services.Interfaces.Logging;
using ShopRite.Application.ValidationServices;  
using ShopRite.Application.ValidationServices.Authentication;
using ShopRite.Domain.Entities.Identity;
using ShopRite.Domain.Interface.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Application.Services.Implementation.Authentication
{
    //***** Note please register this service in the IOContainer
    public class AuthenticationService(ITokenManagement tokenManagement, IUserManagement userManagement,
        IRoleManagement roleManagement, IMapper mapper,
        IAppLogger<AuthenticationService> logger,
        IValidator<CreateUser> createUserValidator
        , IValidator<LoginUser> loginUserValidator,IValidationService validationService) : IAuthenticationService
    {
        public async Task<ServiceResponse<string>> CreateUser(CreateUser createUser)
        {
            #region fluentvalidation
            var _vatidationResu1t = await validationService.ValidateAsync(createUser, createUserValidator);
            if (!_vatidationResu1t.Success)
                return new ServiceResponse<string>(null,System.Net.HttpStatusCode.BadRequest
                    ,_vatidationResu1t.Success,_vatidationResu1t.Message); 
            #endregion

            ApplicationUser appuser = mapper.Map<ApplicationUser>(createUser);
            appuser.UserName = createUser.Email;
            appuser.PasswordHash = createUser.Password;
            appuser.Active = true;
            appuser.Blocked = false;
            appuser.DateExpired = DateTime.UtcNow.AddMonths(3);  


            var result = await userManagement.CreateUserAsync(appuser);
            if (!result)
                return new ServiceResponse<string>(
                    "failed"
                    , System.Net.HttpStatusCode.BadRequest, false
                    , "Email Address might be already in use or unknown error occurredO•");


            #region to-revalidate-usercreation
           var user = await userManagement.GetUserByEmailAsync(createUser.Email); 
            #endregion

            #region Assingrole
            var getalluser = await userManagement.GetAllUsersAsync();
           bool addRoleToUser = await roleManagement.AddUserToRoleAsync(user!, getalluser!.Count() < 1 ? "User" : "Admin");
            #endregion


            if (!addRoleToUser)
            {
                //remove user becos we are unable to add role
                int removeUser = await userManagement.RemoveUserByEmailAsync(user!.Email!);
                if(removeUser <= 0)
                {
                    // error occurred white rolling back changes
                    // then log the error
                    logger.LogError(
                    new Exception($"User with Email as {user.Email} failed to be remove as a result of role assigning issue"),
                    "User could not be assigned Rote");
                    return new ServiceResponse<string>("failed", System.Net.HttpStatusCode.InternalServerError, false,
                    "Error occurred in creating account");
                }
                return new ServiceResponse<string>("failed", System.Net.HttpStatusCode.InternalServerError, false,
                    "Error occurred in creating account");
            }
            //TODO Verify Email
            return new ServiceResponse<string>(
                  "success"
                  , System.Net.HttpStatusCode.Created, true
                  , "User created successfully");


        }

        public async Task<ServiceResponse<LoginResponse>> Login(LoginUser loginUser)
        {
            #region fluentvalidation
            var _vatidationResu1t = await validationService.ValidateAsync(loginUser, loginUserValidator);
            if (!_vatidationResu1t.Success)
                return new ServiceResponse<LoginResponse>(null, System.Net.HttpStatusCode.BadRequest
                    , _vatidationResu1t.Success, _vatidationResu1t.Message);
            #endregion

            ApplicationUser appuser = mapper.Map<ApplicationUser>(loginUser);
            appuser.PasswordHash = loginUser.Password;

            bool loginResult = await userManagement.LoginUserAsync(appuser);
            if (!loginResult)
            {
                return new ServiceResponse<LoginResponse>(null, System.Net.HttpStatusCode.Forbidden
                   , false, "invalid credential");
            }

            ApplicationUser? applicationUser = await userManagement.GetUserByEmailAsync(loginUser.Email);
            var claims = await userManagement.GetUserClaimAsync(applicationUser!.Email!);

            TokenRequirement? tokenRequirement = tokenManagement.GenerateToken(claims);

            string refreshToken = tokenManagement.GenerateRefresToken();


            int addRefresToken = await tokenManagement.AddReFresTokenAsync(applicationUser.Id, refreshToken
                , tokenRequirement.SecurityToken!);

            return addRefresToken <= 0 ? new ServiceResponse<LoginResponse>(null
                , System.Net.HttpStatusCode.InternalServerError, false, "internal error occurred while authenticating")
                : new ServiceResponse<LoginResponse>(new LoginResponse
                (
                  
                   tokenRequirement.Token,
                   applicationUser.UserName,
                   applicationUser.Email,
                   roleManagement.GetUserRoleAsync(applicationUser!.Email!).Result.ToList(),
                   tokenRequirement.SecurityToken!.ValidTo,
                   tokenRequirement.SecurityToken.ValidFrom,
                   refreshToken,
                   DateTime.UtcNow.AddMonths(3)
                ),System.Net.HttpStatusCode.OK,true,"token generated successfully");
        }

        //using refreshToken to generate jwt
        public async Task<ServiceResponse<LoginResponse>> ReviveToken(string refreshToken)
        {
            bool validateTokenResutt = await tokenManagement.ValidateReFresTokenAsync(refreshToken);
            if (!validateTokenResutt)
                return new ServiceResponse<LoginResponse>(null, System.Net.HttpStatusCode.BadRequest
                    , false, "invalid token");
            string userld = await tokenManagement.GetUserIdByReFresTokenAsync(refreshToken);
            ApplicationUser? applicationUser = await userManagement.GetUserByIdAsync(userld);
            var ctaims = await userManagement.GetUserClaimAsync(applicationUser!.Email!);
            TokenRequirement tokenRequirement = tokenManagement.GenerateToken(ctaims);
            string newRefreshToken = tokenManagement.GenerateRefresToken();
          int updateRefresToken = await tokenManagement.UpdateReFresTokenAsync(userld, newRefreshToken, tokenRequirement.SecurityToken!.Id);

            return updateRefresToken <= 0 ? new ServiceResponse<LoginResponse>(null
                , System.Net.HttpStatusCode.InternalServerError, false, "internal error occurred while authenticating")
                : new ServiceResponse<LoginResponse>(new LoginResponse
                (

                   tokenRequirement.Token,
                   applicationUser.UserName,
                   applicationUser.Email,
                   roleManagement.GetUserRoleAsync(applicationUser!.Email!).Result.ToList(),
                   tokenRequirement.SecurityToken!.ValidTo,
                   tokenRequirement.SecurityToken.ValidFrom,
                   refreshToken,
                   DateTime.UtcNow.AddMonths(3)
                ), System.Net.HttpStatusCode.OK, true, "token generated successfully");

        }
    }
}

using EntityFramework.Exceptions.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShopRite.Application.Services;
using ShopRite.Application.Services.Interfaces.Logging;
using ShopRite.Domain.Entities;
using ShopRite.Domain.Entities.Identity;
using ShopRite.Domain.Interface;
using ShopRite.Domain.Interface.Authentication;
using ShopRite.Infrastructure.Data;
using ShopRite.Infrastructure.Middleware;
using ShopRite.Infrastructure.Repository;
using ShopRite.Infrastructure.Repository.Authentiction;
using ShopRite.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using System.Threading.Tasks;

namespace ShopRite.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfraustureService(this IServiceCollection services,
            IConfiguration config)
        {
            var projectOptions = config.GetSection(nameof(ProjectOptions)).Get<ProjectOptions>();

            #region JWT Authentication configuration
            var tokenvalidationParameter = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = projectOptions?.ValidIssuer,
                ValidAudience = projectOptions?.ValidAudiences?[0],
                RequireExpirationTime = true,  //wants the token to expire we can make it false at development time
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(projectOptions.SecreteKey)),
                ClockSkew = TimeSpan.Zero
            };
            services.AddSingleton(tokenvalidationParameter);//i register this in ioc-container to be able to re-use this anywhere

            //registering authentication service to use JwtBearerDefaults
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {

                jwt.TokenValidationParameters = tokenvalidationParameter;
                jwt.Audience = projectOptions?.ValidAudiences?[0];
                jwt.ClaimsIssuer = projectOptions?.ValidIssuer;
                jwt.SaveToken = true;

                #region AppendTokenOnCookies
                //jwt.Events = new JwtBearerEvents
                //{
                //    OnMessageReceived = ctx =>
                //    {
                //        ctx.Request.Cookies.TryGetValue("accessToken", out var accessToken);
                //        if (!string.IsNullOrEmpty(accessToken))
                //            ctx.Token = accessToken;
                //        return Task.CompletedTask;
                //    }
                //};
                #endregion


            });
            #endregion

            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("ShopRiteConnection"),
            sqlOptions =>
            {
                //this ensure the migration look up this connectionstring
                sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                sqlOptions.EnableRetryOnFailure();  //Enable automatic retres for transient failures
            }).UseExceptionProcessor(),
            ServiceLifetime.Scoped);

            services.AddScoped<IGeneric<Product>, GenericRepository<Product>>();
            services.AddScoped<IGeneric<Category>, GenericRepository<Category>>();
            services.AddScoped(typeof(IAppLogger<>), typeof(SerilogLoggerAdapter<>));

            //ApplicationUser  this should have been identityUser but we are extending it 
            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

            }).AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<IUserManagement,UserManagement>();
            services.AddScoped<ITokenManagement, TokenManagement>();
            services.AddScoped<IRoleManagement, RoleManagment>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructureService(this IApplicationBuilder app)
        {
            app.UseMiddleware<LoggingMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            
            return app;
        }
    }
}

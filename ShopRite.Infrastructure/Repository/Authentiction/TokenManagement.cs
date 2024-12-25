using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShopRite.Application.Dto;
using ShopRite.Application.Services;
using ShopRite.Domain.Entities.Identity;
using ShopRite.Domain.Interface.Authentication;
using ShopRite.Infrastructure.Data;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ShopRite.Infrastructure.Repository.Authentiction
{
    //use of primary constructor
    public class TokenManagement : ITokenManagement
    {
        private readonly AppDbContext _context;
        private readonly ProjectOptions _projectOptions;

        public TokenManagement(AppDbContext context, IOptionsMonitor<ProjectOptions> projectOptions)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _projectOptions = projectOptions?.CurrentValue ?? throw new ArgumentNullException(nameof(projectOptions));
        }

        public async Task<int> AddReFresTokenAsync(string userId, string refrestoken, SecurityToken token)
        {
            await RemovedUnusedRefeshTokenAsync(userId);


            _context.RefereshTokens.Add(new RefereshToken()
            {
                UsersId = userId,
                Token = SHA512Converter.GenerateSHA512String(refrestoken),
                DateCreated = DateTime.UtcNow,
                DateExpired = DateTime.UtcNow.AddMonths(3),
                JwtId = token.Id,
                IsUsed = false,
                IsRevoked = false
            });
            return await _context.SaveChangesAsync();
        }

        public TokenRequirement GenerateToken(ClaimsIdentity claimsIdentity)
        {
            TokenRequirement tokenRequirement = new ();


            #region GenerateSecureSecret
            //public const string Issuer = "SecureApiByfcmbdigital";
            //public const string Audience = "SecureApiUser";

            //public const string Secret = "OFRC1j9aaR2BvADxNWlG2pmuD392UfQBZZLM1fuzDEzDlEpSsn+btrpJKd3FfY855OMA9oK4Mc8y48eYUrVUSw==";


            //Important note*******
            //The secret is a base64-encoded string, always make sure to use a secure long string so no one can guess it. ever!.
            //a very recommended approach to use is through the HMACSHA256() class, to generate such a secure secret, you can refer to the below function
            // you can run a small test by calling the GenerateSecureSecret() function to generate a random secure secret once, grab it, and use it as the secret above 
            // or you can save it into appsettings.json file and then load it from them, the choice is yours


            //public static string GenerateSecureSecret()
            //{
            //    var hmac = new HMACSHA256();
            //    return Convert.ToBase64String(hmac.Key);
            //}
            #endregion



           var tokenHandler = new JwtSecurityTokenHandler();


            byte[] key = Convert.FromBase64String(_projectOptions.SecreteKey!);  //convert to byte[] from base64
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            //HmacSha256Signature  the bigger the number the longer the key character length

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                #region CustomeClaims
                Subject = claimsIdentity,
                #endregion


                #region GenericClaims
                Issuer = _projectOptions.ValidIssuer,
                Audience = _projectOptions.ValidAudiences?[0],
                Expires = DateTime.Now.Add(_projectOptions.TokenLifeTime),
                SigningCredentials = signingCredentials,  //this is atually used to sing the token and also to validate the token
                #endregion

            };

            SecurityToken? securitytoken = tokenHandler.CreateToken(tokenDescriptor);

            string token = tokenHandler.WriteToken(securitytoken);

            tokenRequirement.Token = token;
            tokenRequirement.SecurityToken = securitytoken;

            return tokenRequirement;
        }

        public string GenerateRefresToken()
        {
            const int bytesize = 64;
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(bytesize));
           // return RandomString(bytesize);
              
        }

        public List<Claim> GetUserClaimsFromTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken =  tokenHandler.ReadJwtToken(token);
            if (jwtToken == null)
                return [];
            return jwtToken.Claims.ToList();
        }

        public async Task<string> GetUserIdByReFresTokenAsync(string refreshtoken)
            => (await _context.RefereshTokens.FirstOrDefaultAsync(_=>_.Token.Equals(refreshtoken)))!.UsersId;


        public async Task RemovedUnusedRefeshTokenAsync(string userId)
        {
            var refreshToken = _context.RefereshTokens.Where(tk => tk.UsersId == userId && tk.IsRevoked == false && tk.IsUsed == false).FirstOrDefault();
            if (refreshToken != null)
            {

                _context.RefereshTokens.Remove(refreshToken);
                await _context.SaveChangesAsync();

            }
        }

        public async Task<int> UpdateReFresTokenAsync(string userId, string refreshtoken, string jwtId)
        {
          var storedRefershToken = await _context.RefereshTokens.FirstOrDefaultAsync(_=>_.UsersId.Equals(userId));
           if(storedRefershToken == null) 
              return -1;
            storedRefershToken.Token = refreshtoken;
            storedRefershToken.IsRevoked = false;
            storedRefershToken.IsUsed = true;
            storedRefershToken.DateCreated = DateTime.UtcNow;
            storedRefershToken.DateExpired = DateTime.UtcNow.AddMonths(3);
            storedRefershToken.JwtId = jwtId;

            _context.RefereshTokens.Update(storedRefershToken);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateReFresTokenAsync(string refrestoken)
        {
            var refreshToken = await _context.RefereshTokens.FirstOrDefaultAsync(_ => _.Token.Equals(refrestoken));

            return refreshToken != null;
        }

        private  string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCjkDEFGH1JKLMNOPQYRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz" + Guid.NewGuid().ToString().ToUpper();
            return new string(Enumerable.Repeat(chars, length).Select(x => x[random.Next(length)]).ToArray());
        }

    }
}

using ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.SwachhBharat.API.Bll.Repository.Repository
{
    public class Repository:IRepository
    {
        private readonly DevSwachhBharatMainEntities dbMain;
        private readonly IConfiguration _configuration;

        public Repository(DevSwachhBharatMainEntities devSwachhBharatMainEntities, IConfiguration configuration)
        {
            dbMain = devSwachhBharatMainEntities;
            _configuration = configuration;
        }
        public async Task<string> LoginAsync(int AppId)
        {
            var UserId = dbMain.UserInApps.Where(a => a.AppId == AppId).Select(a => a.UserId).FirstOrDefault();
            string Email = string.Empty;
            //var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);
            if (!string.IsNullOrEmpty(UserId))
            {
                Email = dbMain.AspNetUsers.Where(a => a.Id == UserId).Select(a => a.Email).FirstOrDefault();
            }

            if (string.IsNullOrEmpty(Email))
            {
                return null;
            }
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,Email),
                 new Claim("AppId",AppId.ToString()),
                 new Claim("UserName", "Rohit"),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };
            var authSigninkey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(10),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninkey, SecurityAlgorithms.HmacSha256Signature));
            return new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
}

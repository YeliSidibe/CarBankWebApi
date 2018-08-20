using CarBank.WebApi.Models;
using CarBank.WebApi.Services.JWT;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarBank.WebApi.Helpers
{

    public class ClaimIdentityHelper 
    {
        public async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password, bool userHasBeenAuthenticated, IJwtFactory _jwtFactory, UserManager<ApplicationUser> _userManager)
        {
            if (string.IsNullOrEmpty(userName))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            if (userHasBeenAuthenticated)
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
            }
            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}

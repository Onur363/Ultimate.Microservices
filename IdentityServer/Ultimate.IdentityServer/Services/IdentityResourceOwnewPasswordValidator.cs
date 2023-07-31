using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.IdentityServer.Models;

namespace Ultimate.IdentityServer.Services
{
    public class IdentityResourceOwnewPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> userManager;

        public IdentityResourceOwnewPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var existUser = await userManager.FindByEmailAsync(context.UserName);
            if (existUser == null)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Email veya şifreniz yanlış" });
                context.Result.CustomResponse = errors;

                return;
            }
            var passwordCheck = await userManager.CheckPasswordAsync(existUser,context.Password);
            if (!passwordCheck)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Email veya şifreniz yanlış" });
                context.Result.CustomResponse = errors;

                return;
            }

            context.Result = new GrantValidationResult(existUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
        }
    }
}

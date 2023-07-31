using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.WebCore.Models;
using Ultimate.WebCore.Services.Abstract;

namespace Ultimate.WebCore.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityService identityService;

        public AuthController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto signInDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var response = await identityService.SignIn(signInDto);
            if (!response.IsSuccess)
            {
                response.Errors.ForEach(x => ModelState.AddModelError(String.Empty, x));
                return View();

            }
            return RedirectToAction(nameof(Index), "Home");
        }

        public async Task<IActionResult> LogOut()
        {
            //Kullanıcı logout dediğinde Cookie bilgisi silinecek
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //cookie kaldırdığımız için IdentityServer tarafında Revoke işlemini gerçekleştiriyoruz

            await identityService.RevokeRefreshToken();

            return RedirectToAction(nameof(HomeController.Index),"Home");

        }
    }
}

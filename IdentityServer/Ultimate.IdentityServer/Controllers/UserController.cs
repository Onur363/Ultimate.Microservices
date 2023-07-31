using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.IdentityServer.Dto;
using Ultimate.IdentityServer.Models;
using Ultimate.SharedCommon.Dtos;
using static IdentityServer4.IdentityServerConstants;

namespace Ultimate.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            var user = new ApplicationUser()
            {
                UserName = signUpDto.UserName,
                City = signUpDto.City,
                Email = signUpDto.Email,
            };

            var result = await userManager.CreateAsync(user, signUpDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(Response<NoContent>.Fail(result.Errors.Select(x => x.Description).ToList(), 400));
            }
            return NoContent();
        }

        [HttpGet("getuser")]
        public async Task<IActionResult> GetUser()
        {   
            //Burada İdentitResource(Config Dosyası içinde tanımladığıöız OpenId yani token alan kullanıcının Id si varsa userExist e dahil ediliyor
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);

            if(userIdClaim == null)
            {
                return BadRequest();
            }

            var user =await userManager.FindByIdAsync(userIdClaim.Value);
            if (user == null)
            {
                return BadRequest();
            }

            return Ok(new { Id = user.Id, Name = user.UserName, Email = user.Email, City = user.City });
        }
    }
}

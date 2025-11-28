using EzShop.Api.DTOs;
using EzShop.Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EzShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var user = new IdentityUser
            {
                Email = registerRequestDTO.UserName,
                UserName = registerRequestDTO.UserName
            };
            var identityUser=await userManager.CreateAsync(user, registerRequestDTO.Password);
            if (identityUser.Succeeded)
            {
                if(registerRequestDTO.Roles!=null && registerRequestDTO.Roles.Any())
                {
                    identityUser=await userManager.AddToRolesAsync(user, registerRequestDTO.Roles);
                    if (identityUser.Succeeded)
                    {
                        return Ok("Registration Successful! Please login.");
                    }
                }
            }
            return BadRequest("Registration Failed! Please try again.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDTO loginRequestDTO)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDTO.UserName);

            if (user != null)
            {
                var checkPassword=await userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
                if (checkPassword)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var jwtToken = tokenRepository.CreateJwtToken(user, roles.ToList());
                        return Ok(jwtToken);
                    }
                    
                }
            }

            return BadRequest("User Name or Password is incorrect.");
        }
    
    }
}

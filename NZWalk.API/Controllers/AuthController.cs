using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalk.API.Models.Dto;
using NZWalk.API.Models.DTO;
using NZWalk.API.Repositories;

namespace NZWalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository) 
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }


        // POST : /api/Auth/Register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto) 
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
            if (identityResult.Succeeded)
            {
                // Add role to this user
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered! Please login");
                    }
                }

            }

            return BadRequest("Something went wrong!!");
        }

        // POST: /api/Auth/Login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto) 
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    //Get Role from User

                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null) 
                    {
                        //create token

                        var jwttoken = tokenRepository.CreateJWTToken(user,roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwttoken,
                        };

                        return Ok(response);
                    }

                }
            }

            return BadRequest("Username or password incorrect");

        }
    }
}

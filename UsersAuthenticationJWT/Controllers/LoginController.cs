using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsersAuthenticationJWT.Dto;
using UsersAuthenticationJWT.Services.UserAuthentication;

namespace UsersAuthenticationJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly IUserAuthenticationService UserService;

        public LoginController(
            IConfiguration config, 
            IUserAuthenticationService userService)
        {
            this.Configuration = config;
            this.UserService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginRequest loginRequest)
        {
            var userAuthenticationResult = await this.UserService.Authenticate(loginRequest.UserName, loginRequest.Password, this.HttpContext.RequestAborted);

            if (userAuthenticationResult == null)
            {
                return Unauthorized();
            }

            var token = this.UserService.GenerateJwtSecurityToken(userAuthenticationResult);

            return Ok(token);
        }
    }
}
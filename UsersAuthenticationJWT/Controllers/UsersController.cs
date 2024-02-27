using Microsoft.AspNetCore.Mvc;
using UsersAuthenticationJWT.Services.UserAuthentication;

namespace UsersAuthenticationJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserAuthenticationService UserService;

        public UsersController(IUserAuthenticationService userService)
        {
            this.UserService = userService;
        }

        /// <summary>
        /// Get the user by the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get([FromRoute]Guid id)
        {
            // this.UserService.Get
            return Ok(null);
        }
    }
}

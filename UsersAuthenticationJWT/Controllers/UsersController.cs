using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using UsersAuthenticationJWT.Entities;
using UsersAuthenticationJWT.Services.Encryption;
using UsersAuthenticationJWT.Services.UserAuthentication;
using UsersAuthenticationJWT.Services.Users;
using UsersAuthenticationJWT.Services.Users.Exceptions;

namespace UsersAuthenticationJWT.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository UserService;
        private readonly IEncryptionService EncryptionService;

        public UsersController(IUserRepository userService, IEncryptionService encryptionService)
        {
            this.UserService = userService;
            this.EncryptionService = encryptionService;
        }

        /// <summary>
        /// Get all the users. The passwords are not included.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int skip,
            [FromQuery] int take)
        {
            try
            {
                var users = await this.UserService.GetUsersAsync(null, skip, take, this.HttpContext.RequestAborted);
                return Ok(users);
            }
            catch (PaginationExceededException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get the user by the id. The password is not included.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get([FromRoute]Guid id)
        {
            var user = await this.UserService.GetUserByIdAsync(id, this.HttpContext.RequestAborted);

            if (user == null)
            {
                NotFound(id);
            }

            return Ok(user);
        }

        /// <summary>
        /// Insert a new user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] UserDto user)
        {
            var result = await this.UserService.AddUserAsync(user.ToUser(this.EncryptionService));
            return Ok(result);
        }


        /// <summary>
        /// Update the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(
            [FromBody] UserDto user)
        {
            await this.UserService.UpdateUserAsync(user.ToUser(this.EncryptionService));
            return Ok();
        }
    }

    public class UserDto
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// The UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// User roles
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Converts into a Entities.User
        /// </summary>
        /// <param name="encryptionService"></param>
        /// <returns></returns>
        public User ToUser(IEncryptionService encryptionService)
        {
            return new User()
            {
                Id = this.Id,
                UserName = this.UserName,
                EncryptedPassword = !string.IsNullOrEmpty(this.Password) ? encryptionService.Encrypt(this.Password) : null,
                Role = this.Role
            };
        }
    }
}

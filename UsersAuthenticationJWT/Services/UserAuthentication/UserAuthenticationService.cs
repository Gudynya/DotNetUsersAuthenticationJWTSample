using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsersAuthenticationJWT.Entities;
using UsersAuthenticationJWT.Services.Encryption;
using UsersAuthenticationJWT.Services.UserAuthentication;
using UsersAuthenticationJWT.Services.Users;

namespace UsersAuthenticationJWT.Services.Authentication
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IConfiguration Configuration;
        private readonly IEncryptionService EncryptionService;
        private readonly IUserRepository UserRepository;

        public UserAuthenticationService(
            IConfiguration configuration,
            IEncryptionService encryptionService,
            IUserRepository userRepository) {
            
            this.Configuration = configuration;
            this.EncryptionService = encryptionService;
            this.UserRepository = userRepository;
        }

        public async Task<User> GetUserName(string userName, CancellationToken cancellationToken = default)
        {
            var users = await this.UserRepository.GetUsersAsync(x => x.UserName == userName, 0, 1, cancellationToken);
            return users.FirstOrDefault();
        }

        public async Task<UserAuthenticationResult?> Authenticate(string userName, string password, CancellationToken cancellationToken = default)
        {
            var user = await this.GetUserName(userName, cancellationToken);
            
            if(user == null || user.Id == null || user.Id == default)
            {
                return null;
            }

            var encryptedPassword = this.EncryptionService.Encrypt(password);

            if (user.EncryptedPassword != encryptedPassword)
            {
                return null;
            }

            return new UserAuthenticationResult()
            {   
                Id = user.Id.Value,
                Role = user.Role,
            };
        }

        public string GenerateJwtSecurityToken(UserAuthenticationResult userAuthenticationResult)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userAuthenticationResult.Id.ToString()),
                new Claim(ClaimTypes.Role, userAuthenticationResult.Role),
                // You can add more claims as needed
            };

            var Sectoken = new JwtSecurityToken(Configuration["Jwt:Issuer"],
              Configuration["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }
    }
}

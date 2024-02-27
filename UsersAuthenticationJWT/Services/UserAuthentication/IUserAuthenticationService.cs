using UsersAuthenticationJWT.Entities;

namespace UsersAuthenticationJWT.Services.UserAuthentication
{
    public interface IUserAuthenticationService
    {
        /// <summary>
        /// Get the User using the username
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<User> GetUserName(string userName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if the user exists using the username and the password.
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="cancellationToken">Abort CancelationToken</param>
        /// <returns></returns>
        Task<UserAuthenticationResult?> Authenticate(string userName, string password, CancellationToken cancellationToken = default);

        /// <summary>
        /// Generate a JWT Token
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        string GenerateJwtSecurityToken(UserAuthenticationResult userAuthenticationResult);
    }
}

using UsersAuthenticationJWT.Entities;
using UsersAuthenticationJWT.Services.Users.Exceptions;

namespace UsersAuthenticationJWT.Services.Users
{
    /// <summary>
    /// User repository
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get the users
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<User>> GetUsersAsync(
            Func<User, bool> predicate, int skip, int take, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a user by the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> AddUserAsync(User user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update a existing user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task UpdateUserAsync(User user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

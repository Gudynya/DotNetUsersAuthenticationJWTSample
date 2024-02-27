using System.Collections.Concurrent;
using UsersAuthenticationJWT.Entities;

namespace UsersAuthenticationJWT.Services.Storage
{
    /// <summary>
    /// Storage Service
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Get the users storage (like a database)
        /// </summary>
        /// <returns></returns>
        ConcurrentDictionary<Guid, User> GetUsersStorage();
    }
}

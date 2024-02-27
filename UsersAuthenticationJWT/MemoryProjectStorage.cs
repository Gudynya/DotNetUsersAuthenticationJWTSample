using System.Collections.Concurrent;

namespace UsersAuthenticationJWT
{
    public static class MemoryProjectStorage
    {
        internal static ConcurrentDictionary<Guid, Entities.User> Users;
    }
}

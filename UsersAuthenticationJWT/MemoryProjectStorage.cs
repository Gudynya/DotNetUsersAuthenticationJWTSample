using System.Collections.Concurrent;
using UsersAuthenticationJWT.Services.Users;

namespace UsersAuthenticationJWT
{
    public static class MemoryProjectStorage
    {
        internal static IDictionary<string, User> Users = new ConcurrentDictionary<string, User>();
    }
}

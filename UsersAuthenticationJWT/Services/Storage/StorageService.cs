using System.Collections.Concurrent;
using UsersAuthenticationJWT.Entities;
using UsersAuthenticationJWT.Services.Encryption;

namespace UsersAuthenticationJWT.Services.Storage
{
    public class StorageService : IStorageService
    {
        public StorageService(IEncryptionService encryptionService) { 
            if (MemoryProjectStorage.Users == null)
            {
                var id = Guid.NewGuid();
                MemoryProjectStorage.Users = new System.Collections.Concurrent.ConcurrentDictionary<Guid, Entities.User>();
                MemoryProjectStorage.Users.TryAdd(id, new Entities.User()
                {
                    Id = id,
                    EncryptedPassword = encryptionService.Encrypt("toor"),
                    UserName = "root",
                    Role = "Admin",
                });
            }
        }

        public ConcurrentDictionary<Guid, User> GetUsersStorage()
        {
            return MemoryProjectStorage.Users;
        }
    }
}

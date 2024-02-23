using System.Collections;
using System.Collections.Concurrent;
using UsersAuthenticationJWT.Services.Encryption;

namespace UsersAuthenticationJWT.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IDictionary<string, User> Users;
        private readonly IEncryptionService EncryptionService;

        public UserService(IEncryptionService encryptionService) { 
            Users = new ConcurrentDictionary<string, User>();
            
            MemoryProjectStorage.Users.TryAdd("root", new User()
            {
                Id = Guid.NewGuid(),
                UserName = "root",
                EncryptedPassword = encryptionService.Encrypt("toor"),
                Role = "admin"
            });

            this.EncryptionService = encryptionService;
        }

        public User GetUserName(string userName)
        {
            MemoryProjectStorage.Users.TryGetValue(userName, out var user);
            return user;
        }

        public bool CheckUserPassword(string userName, string password, out User? user)
        {
            user = this.GetUserName(userName);
            
            if(user == null)
            {
                return false;
            }

            var encryptedPassword = this.EncryptionService.Encrypt(password);

            if (user.EncryptedPassword != encryptedPassword)
            {
                user = null;
                return false;
            }

            return true;
        }
    }
}

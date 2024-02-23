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
            
            this.Users.TryAdd("root", new User()
            {
                UserName = "root",
                EncryptedPassword = encryptionService.Encrypt("toor")
            });

            this.EncryptionService = encryptionService;
        }

        public User GetUserName(string userName)
        {
            Users.TryGetValue(userName, out var user);
            return user;
        }

        public bool CheckUserPassword(string userName, string password)
        {
            var user = GetUserName(userName);
            
            if(user == null)
            {
                return false;
            }

            var encryptedPassword = this.EncryptionService.Encrypt(password);

            return user.EncryptedPassword == encryptedPassword;
        }
    }
}

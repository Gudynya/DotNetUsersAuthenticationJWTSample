using System.Text.Json.Serialization;

namespace UsersAuthenticationJWT.Services.Users
{
    /// <summary>
    /// User's DTO
    /// </summary>
    public class User
    {
        /// <summary>
        /// The UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The encryptedPassword
        /// </summary>
        [JsonIgnore]
        public string EncryptedPassword { get; set; }
    }
}

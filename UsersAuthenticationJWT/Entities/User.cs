using System.Text.Json.Serialization;

namespace UsersAuthenticationJWT.Entities
{
    /// <summary>
    /// User's DTO
    /// </summary>
    public class User
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The encryptedPassword
        /// </summary>
        [JsonIgnore]
        public string EncryptedPassword { get; set; }

        /// <summary>
        /// User roles
        /// </summary>
        public string Role { get; set; }
    }
}

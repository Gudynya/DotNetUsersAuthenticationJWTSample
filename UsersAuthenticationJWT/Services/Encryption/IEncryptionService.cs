namespace UsersAuthenticationJWT.Services.Encryption
{
    /// <summary>
    /// Encryption Service
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// Encrypt 
        /// </summary>
        /// <returns></returns>
        public string Encrypt(string value, string region = "default");

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Decrypt(string value, string region = "default");
    }
}

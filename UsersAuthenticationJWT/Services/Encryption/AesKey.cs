using System.Text;

namespace UsersAuthenticationJWT.Services.Encryption
{
    /// <summary>
    /// A class to store the Aes Key and IV
    /// </summary>
    public class AesKey
    {
        public AesKey(string keyRaw, string ivRaw)
        {
            this.Key = Encoding.UTF8.GetBytes(keyRaw);
            this.IV = Encoding.UTF8.GetBytes(ivRaw);
        }

        public byte[] Key { get; set; }

        public byte[] IV { get; set; }
    }
}

using System.Text;

namespace UsersAuthenticationJWT.Services.Encryption
{
    public class KeyStorage
    {
        public KeyStorage(string keyRaw, string ivRaw)
        {
            this.Key = Encoding.UTF8.GetBytes(keyRaw);
            this.IV = Encoding.UTF8.GetBytes(ivRaw);
        }

        public byte[] Key { get; set; }

        public byte[] IV { get; set; }
    }
}

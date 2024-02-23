using System.Security.Cryptography;

namespace UsersAuthenticationJWT.Utils
{
    public static class RandomKeyGenerator
    {
        public static string GenerateRandomKey(int keySize)
        {
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[keySize / 8]; // keySize is in bits, so we divide by 8 to get bytes
                rngCsp.GetBytes(randomBytes);
                return BitConverter.ToString(randomBytes).Replace("-", "").ToLower();
            }
        }

        public static string GenerateRandomIV()
        {
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[16]; // IV size for AES is typically 128 bits (16 bytes)
                rngCsp.GetBytes(randomBytes);
                return BitConverter.ToString(randomBytes).Replace("-", "").ToLower();
            }
        }
    }
}

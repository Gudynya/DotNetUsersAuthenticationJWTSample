using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace UsersAuthenticationJWT.Services.Encryption
{
    public class EncryptionService : IEncryptionService
    {
        private readonly IConfiguration configuration;

        private readonly ConcurrentDictionary<string, KeyStorage> Cache = new ConcurrentDictionary<string, KeyStorage>();

        public EncryptionService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Encrypt(string plainText, string region = "default")
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = this.GetKey(region).Key;
                aesAlg.IV = this.GetKey(region).IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string Decrypt(string cipherText, string region = "default")
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = this.GetKey(region).Key;
                aesAlg.IV = this.GetKey(region).IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        public KeyStorage GetKey(string region = "default")
        {
            if(this.Cache.TryGetValue(region, out var key))
            {
                return key;
            }

            this.Cache.TryAdd(region, 
                new KeyStorage(
                    configuration[$"Encryption:{region}:Key"], 
                    configuration[$"Encryption:{region}:IV"]));


            return this.GetKey(region);
        }
    }
}

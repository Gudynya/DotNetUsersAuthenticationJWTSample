using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace UsersAuthenticationJWT.Services.Encryption
{
    /// <summary>
    /// Simple encryption service
    /// </summary>
    public class EncryptionService : IEncryptionService
    {
        /// <summary>
        /// Configuration
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// A single cache to store the Aes Keys
        /// </summary>
        private readonly ConcurrentDictionary<string, AesKey> Cache = new ConcurrentDictionary<string, AesKey>();

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="configuration"></param>
        public EncryptionService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Serialize and encrypt a object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="data">Object to encrypt</param>
        /// <param name="region">Setting region</param>
        /// <returns>Encrypted value in string</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public string Encrypt<T>(T data, string region = "default")
            where T: class
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var serializedData = System.Text.Json.JsonSerializer.Serialize(data);
            return this.Encrypt(serializedData, region);
        }

        /// <summary>
        /// Encrypt a simple data
        /// </summary>
        /// <param name="data">Data to encrypt</param>
        /// <param name="region">Setting region</param>
        /// <returns>Encrypted value in string</returns>
        public string Encrypt(string data, string region = "default")
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
                            swEncrypt.Write(data);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        /// <summary>
        /// Decrypt and deserialize in a class
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="data">Encrypted value</param>
        /// <param name="region">Setting region</param>
        /// <returns></returns>
        public T? Decrypt<T>(string data, string region = "default")
            where T : class
        {
            var decryptedData = this.Decrypt(data, region);

            if (decryptedData == null)
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Deserialize<T>(decryptedData);
        }

        /// <summary>
        /// Decrypt a value
        /// </summary>
        /// <param name="data">Encrypted value</param>
        /// <param name="region">Setting region</param>
        /// <returns></returns>
        public string Decrypt(string data, string region = "default")
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = this.GetKey(region).Key;
                aesAlg.IV = this.GetKey(region).IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(data)))
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

        /// <summary>
        /// Get the key from the storage
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        private AesKey GetKey(string region = "default")
        {
            if(this.Cache.TryGetValue(region, out var key))
            {
                return key;
            }

            this.Cache.TryAdd(region, 
                new AesKey(
                    configuration[$"Encryption:{region}:Key"], 
                    configuration[$"Encryption:{region}:IV"]));


            return this.GetKey(region);
        }
    }
}

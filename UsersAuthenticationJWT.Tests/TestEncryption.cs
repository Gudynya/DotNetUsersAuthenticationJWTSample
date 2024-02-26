using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;
using UsersAuthenticationJWT.Controllers;
using UsersAuthenticationJWT.Dto;
using UsersAuthenticationJWT.Services.Encryption;

namespace UsersAuthenticationJWT.Tests
{
    /// <summary>
    /// Testing for the encryption service
    /// </summary>
    public class TestEncryption : TestBase
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="factory"></param>
        public TestEncryption(WebApplicationFactory<Startup> factory) 
            : base(factory)
        {
        }

        /// <summary>
        /// Encrypts and decrypts a simple string using the current WebApplication Encryption Service
        /// </summary>
        [Fact]
        public async void TestEncryptDecrypt()
        {
            var encryptionService = this.Factory.Services.GetRequiredService<IEncryptionService>();

            var text = "THIS IS A TEXT TO ENCRYPT AND DECRYPT";

            var encryptedText = encryptionService.Encrypt(text);
            Assert.NotEqual(text, encryptedText);

            var decryptedText = encryptionService.Decrypt(encryptedText);
            Assert.Equal(text, decryptedText);
        }
    }
}
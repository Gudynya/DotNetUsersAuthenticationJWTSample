using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;
using UsersAuthenticationJWT.Controllers;
using UsersAuthenticationJWT.Dto;
using UsersAuthenticationJWT.Services.Encryption;

namespace UsersAuthenticationJWT.Tests
{
    public class TestEncryption : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> Factory;

        public TestEncryption(WebApplicationFactory<Startup> factory)
        {
            this.Factory = factory;
        }

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
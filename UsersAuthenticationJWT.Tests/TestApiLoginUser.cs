using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using UsersAuthenticationJWT.Controllers;
using UsersAuthenticationJWT.Dto;

namespace UsersAuthenticationJWT.Tests
{
    public class TestApiLoginUser : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> Factory;

        public TestApiLoginUser(WebApplicationFactory<Startup> factory)
        {
            this.Factory = factory;
        }

        [Fact]
        public async void TestNotLoggedUser()
        {
            var client = this.Factory.CreateClient();
            var response = await client.GetAsync($"api/HelloWorld");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void TestLoginUser()
        {
            var client = this.Factory.CreateClient();

            var requestBody = System.Text.Json.JsonSerializer.Serialize(new LoginRequest()
            {
                UserName = "root",
                Password = "toor",
                Device = "Testing"
            });

            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync($"api/Login", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            string result = await response.Content.ReadAsStringAsync();
            
            Assert.NotNull(result);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result);

            response = await client.GetAsync($"api/HelloWorld");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
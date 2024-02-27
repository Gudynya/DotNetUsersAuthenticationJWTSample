using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;
using System.Text.Json;
using UsersAuthenticationJWT.Controllers;
using UsersAuthenticationJWT.Dto;
using UsersAuthenticationJWT.Entities;
using UsersAuthenticationJWT.Services.Storage;
using static UsersAuthenticationJWT.Controllers.UsersController;

namespace UsersAuthenticationJWT.Tests
{
    public class TestApiUser : TestBase
    {
        protected HttpClient client;

        public TestApiUser(WebApplicationFactory<Startup> factory)
            : base(factory)
        {
            if (client != null)
            {
                return;
            }

            client = this.Factory.CreateClient();
            var requestBody = System.Text.Json.JsonSerializer.Serialize(new LoginRequest()
            {
                UserName = "root",
                Password = "toor",
                Device = "Testing"
            });

            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = client.PostAsync($"api/Login", content).Result;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            string result = response.Content.ReadAsStringAsync().Result;

            Assert.NotNull(result);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result);
        }

        [Fact]
        public async void TestApiGetUsers()
        {
            var response = await client.GetAsync($"api/Users?skip=0&take=20");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            var users = JsonSerializer.Deserialize<List<User>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            Assert.NotNull(users);
            Assert.NotEmpty(users);
            Assert.Single(users);

            response = await client.GetAsync($"api/Users/{users.First().Id.ToString()}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            result = await response.Content.ReadAsStringAsync();

            var user = JsonSerializer.Deserialize<User>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(user);
            Assert.Equal(user.Id, users.First().Id);
        }

        [Fact]
        public async void TestPostApiUsers()
        {
            var requestBody = System.Text.Json.JsonSerializer.Serialize(new UserDto()
            {
                UserName = "user",
                Role = "Admin",
                Password = "password"
            });

            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"api/Users", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();

            var userId = JsonSerializer.Deserialize<Guid>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.False(userId == default);

            var storage = this.Factory.Services.GetRequiredService<IStorageService>();
            Assert.True(storage.GetUsersStorage().TryGetValue(userId, out _));
        }

        [Fact]
        public async void TestPutApiUsers()
        {
            var storage = this.Factory.Services.GetRequiredService<IStorageService>();

            var userId = Guid.NewGuid();
            var user = new User()
            {
                Id = userId,
                UserName = "UserToPut",
                Role = "Admin"
            };

            Assert.True(storage.GetUsersStorage().TryAdd(userId, user));

            Assert.NotNull(user);
            Assert.False(user.Id == default);

            var requestBody = System.Text.Json.JsonSerializer.Serialize(new UserDto()
            {
                Id = userId,
                UserName = user.UserName,
                Password = "Password",
                Role = "Admin2",
            });

            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"api/Users", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.True(storage.GetUsersStorage().TryGetValue(user.Id.Value, out var userInStorage));

            Assert.Equal("Admin2", userInStorage.Role);
        }
    }
}
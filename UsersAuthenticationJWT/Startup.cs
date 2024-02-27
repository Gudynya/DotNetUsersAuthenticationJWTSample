using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UsersAuthenticationJWT.Jwt;
using UsersAuthenticationJWT.Services.Authentication;
using UsersAuthenticationJWT.Services.Encryption;
using UsersAuthenticationJWT.Services.Storage;
using UsersAuthenticationJWT.Services.UserAuthentication;
using UsersAuthenticationJWT.Services.Users;

namespace UsersAuthenticationJWT
{
    /// <summary>
    /// The program startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="builder"></param>
        public Startup(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddSingleton<IStorageService, StorageService>();
            builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
            builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Jwt configuration starts here
            var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
            var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

            if (string.IsNullOrWhiteSpace(jwtIssuer))
            {
                throw new NullReferenceException(nameof(jwtIssuer));
            }

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new NullReferenceException(nameof(jwtKey));
            }

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = jwtIssuer,
                         ValidAudience = jwtIssuer,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                     };
                     options.Events = new CustomJwtBearerEvent();
                 });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

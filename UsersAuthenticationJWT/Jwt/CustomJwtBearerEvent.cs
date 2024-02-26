using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace UsersAuthenticationJWT.Jwt
{
    /// <summary>
    /// Custom JWT Bearer Events
    /// </summary>
    public class CustomJwtBearerEvent : JwtBearerEvents
    {
        public override Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            return base.AuthenticationFailed(context);
        }

        public override Task TokenValidated(TokenValidatedContext context)
        {
            return base.TokenValidated(context);
        }
    }
}

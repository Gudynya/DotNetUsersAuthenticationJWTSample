using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAuthenticationJWT.Tests
{
    internal static class TestConfigurationVariables
    {
        public static Dictionary<string, string> DEFAULT_ENVIROMENT_VARIABLES = new Dictionary<string, string>
            {
                { "Jwt:Issuer", "Test JWT Builder" },
                { "JWT:KEY", "wx7K_foP9uZUWIgNVGqY2qQ2P5295fAo7vlqwJoBRThz4upUlMdmGq_L4bcxeOMR9q1Bj2W8P3wy0XyYTg3tXhagEo-cFOUfSRgrDSMMhcXOnuDrxKarJdEJe2VRSZ1064qEXyc8GOq_D-4zSqQrRYz-LWQpVfQ67cAXKf1Ggsc" },
                { "ENCRYPTION:DEFAULT:KEY", "0123456789ABCDEF" },
                { "ENCRYPTION:DEFAULT:IV", "0123456789ABCDEF" }
            };
    }
}

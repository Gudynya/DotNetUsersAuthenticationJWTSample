using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAuthenticationJWT.Tests
{
    public class TestBase : IClassFixture<WebApplicationFactory<Startup>>
    {
        /// <summary>
        /// Web Application Factory
        /// </summary>
        protected readonly WebApplicationFactory<Startup> Factory;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="factory"></param>
        protected TestBase(WebApplicationFactory<Startup> factory)
        {
            this.Factory = factory.WithWebHostBuilder(builder =>
            {
                this.DeclareVariables(builder);
            });
        }

        /// <summary>
        /// Declare the Enviroment Variables
        /// </summary>
        /// <param name="builder"></param>
        internal void DeclareVariables(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddInMemoryCollection(TestConfigurationVariables.DEFAULT_ENVIROMENT_VARIABLES);
            });

            foreach (var variable in TestConfigurationVariables.DEFAULT_ENVIROMENT_VARIABLES)
            {
                // Set the Jwt:Key configuration value in the IWebHostBuilder
                builder.UseSetting(variable.Key, variable.Value);
            }
        }
    }
}

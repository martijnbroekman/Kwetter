using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetter.WebApplication.Configuration
{
    public static class SwaggerServiceExstensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IHostingEnvironment hostingEnvironment)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Info { Title = "Kwetter API v1.0", Version = "v1.0" });

                c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "password",
                    TokenUrl = Path.Combine(hostingEnvironment.WebRootPath, "/connect/token"),
                    // Optional scopes
                    //Scopes = new Dictionary<string, string>
                    //{
                    //    { "api-name", "my api" },
                    //}
                    
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "oauth2", new string[] { } }
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Versioned API v1.0");
            });

            return app;
        }
    }
}

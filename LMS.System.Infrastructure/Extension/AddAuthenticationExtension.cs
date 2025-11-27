using System;
using System.Collections.Generic;
using System.Text;
using LMS.System.Infrastructure.Authenticates;
using LMS.System.Infrastructure.Authenticates.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace LMS.System.Infrastructure.Extension
{
    /// <summary>
    /// Static class containing extension methods for authentication configuration.
    /// </summary>
    public static class AddAuthenticationExtension
    {
        /// <summary>
        /// Extension method to configure JWT Bearer authentication for the application.
        /// </summary>
        /// <param name="service">The IServiceCollection to which we add authentication services.</param>
        /// <param name="configure">Application configuration root.</param>
        public static void AddJWTBearerExtension(this IServiceCollection service, IConfiguration configure)
        {
            // Get the "Authentication" section from application configuration.
            var section = configure.GetSection("Authentication");

            // Bind Authentication section to AuthOptions class and register it with DI container.
            service.Configure<AuthenticationOptions>(section);

            // Instantiate AuthOptions instance from configuration section.
            var authOptions = section.Get<AuthenticationOptions>();

            // Configure authentication services with JWT Bearer as default scheme.
            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        // Configure token validation parameters.
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            // Expected token issuer.
                            ValidateIssuer = true,
                            ValidIssuer = authOptions?.Issuer,

                            // Expected token audience
                            ValidateAudience = true,
                            ValidAudience = authOptions?.Audience,

                            // Check token expiration
                            ValidateLifetime = true,

                            // Configure signing key (using HS256 algorithm).
                            IssuerSigningKey = authOptions?.GetSymmetricSecurityKey(),

                            // Verify token signature.
                            ValidateIssuerSigningKey = true,
                        };

                        // Customize JWT Bearer events.
                        options.Events = new JwtBearerEvents
                        {
                            // Extract JWT token from HTTP-only cookies instead of Authorization header.
                            OnMessageReceived = context =>
                            {
                                // Retrieve token from cookie.
                                var token = context.Request.Cookies[TokensConst.GetTokenName];
                                context.Token = token;
                                return Task.CompletedTask;
                            },
                        };
                    });
        }
    }
}

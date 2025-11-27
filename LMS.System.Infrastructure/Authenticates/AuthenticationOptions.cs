using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LMS.System.Infrastructure.Authenticates
{
    /// <summary>
    /// Configuration class for JWT authentication settings.
    /// Contains all necessary parameters for token generation and validation.
    /// </summary>
    public class AuthenticationOptions
    {
        /// <summary>
        /// Token issuer identifier.
        /// </summary>
        public required string Issuer { get; set; }

        /// <summary>
        /// Intended token recipient.
        /// </summary>
        public required string Audience { get; set; }

        /// <summary>
        /// Signing key for tokens.
        /// </summary>
        public required string Secret { get; set; }

        /// <summary>
        /// oken expiration time in minutes.
        /// </summary>
        public int TokenLifeTime { get; set; }

        /// <summary>
        /// Creates symmetric security key from secret string.
        /// </summary>
        /// <returns>SymmetricSecurityKey instance created from the secret.</returns>
        public SymmetricSecurityKey GetSymmetricSecurityKey()
            => new(Encoding.ASCII.GetBytes(Secret));
    }
}

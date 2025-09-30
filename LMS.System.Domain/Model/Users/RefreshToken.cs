using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.System.Domain.Model.Users
{
    /// <summary>
    /// Refresh token entity for storing refresh tokens in DB.
    /// </summary>
    public class RefreshToken : Entity
    {
        /// <summary>
        /// Token identifier.
        /// </summary>
        public required string Token { get; set; }

        /// <summary>
        /// Expiration date and time of the token.
        /// </summary>
        public required DateTime Expires { get; set; }

        /// <summary>
        /// Indicates if the token has been revoked.
        /// </summary>
        public bool IsRevoked { get; set; }

        /// <summary>
        /// User ID associated with the token.
        /// </summary>
        public required Guid UserId { get; set; }

        /// <summary>
        /// Navigation property to the user.
        /// </summary>
        public User User { get; set; } = null!;

        /// <summary>
        /// Checks if the token is expired.
        /// </summary>
        public bool IsExpired => DateTime.UtcNow >= Expires;

        /// <summary>
        /// Checks if the token is active (not expired and not revoked).
        /// </summary>
        public bool IsActive => !IsExpired && !IsRevoked;
    }
}

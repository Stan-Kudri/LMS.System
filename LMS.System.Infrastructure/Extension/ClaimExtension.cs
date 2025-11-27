using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace LMS.System.Infrastructure.Extension
{
    /// <summary>
    /// Getting data from Claims.
    /// </summary>
    public static class ClaimExtension
    {
        /// <summary>
        /// Getter user id by Claims Principal.
        /// </summary>
        /// <param name="claimsPrincipal">ClaimsPrincipal access token.</param>
        /// <returns>User id by ClaimsPrincipal.</returns>
        public static Guid? TryParseIdUserClaimsPrincipal(this ClaimsPrincipal? claimsPrincipal)
        {
            if (claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString)
            {
                return null;
            }

            if (!Guid.TryParse(userIdString, out var userId))
            {
                return null;
            }

            return userId;
        }
    }
}

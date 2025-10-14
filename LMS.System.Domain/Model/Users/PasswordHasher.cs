using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.System.Domain.Model.Users
{
    /// <summary>
    /// Service for password hashing and verification using BCrypt algorithm.
    /// </summary>
    public class PasswordHasher
    {
        /// <summary>
        /// Generates a password hash using enhanced BCrypt algorithm.
        /// </summary>
        /// <param name="password">Password in plain text.</param>
        /// <returns>Password hash in BCrypt format.</returns>
        public string Generate(string password)
            => BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        /// <summary>
        /// Verifies if a password matches its hash.
        /// </summary>
        /// <param name="password">Password in plain text to verify.</param>
        /// <param name="passwordHash">Password hash from the database.</param>
        /// <returns>True if password matches the hash; otherwise false.</returns>
        public bool Verification(string password, string passwordHash)
            => BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
    }
}

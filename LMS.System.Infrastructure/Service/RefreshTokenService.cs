using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.System.Domain.DB;
using LMS.System.Domain.Model.Users;
using Microsoft.EntityFrameworkCore;

namespace LMS.System.Infrastructure.Service
{
    /// <summary>
    /// Service for managing refresh tokens in the application.
    /// Handles operations like creating, updating, removing and validating refresh tokens.
    /// </summary>
    /// <param name="appDbContext">Database context for data access.</param>
    /// <param name="tokenService">Service for token generation operations.</param>
    public class RefreshTokenService(AppDbContext appDbContext, JwtTokenService tokenService)
    {
        /// <summary>
        /// Creates or updates a refresh token for a user.
        /// If the token already exists for the user, no action is taken.
        /// </summary>
        /// <param name="refreshToken">The refresh token to upsert.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <exception cref="ArgumentException">Thrown when the specified user doesn't exist.</exception>
        /// <returns>A <see cref="Task"/> Representing the asynchronous operation.</returns>
        public async Task UpsertAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            // Validate that the user exists
            if (!appDbContext.Users.Any(e => e.Id == refreshToken.UserId))
            {
                throw new ArgumentException("This user does not exist.");
            }

            // Check if token already exists for this user
            var isTokenUsed = appDbContext.RefreshTokens.Any(e => e.UserId == refreshToken.UserId
                                                                  && e.Id == refreshToken.Id
                                                                  && e.IsActive
                                                                  && e.Expires <= DateTime.UtcNow);

            // If token exists, exit without changes
            if (isTokenUsed)
            {
                return;
            }

            // Add new token and save changes
            await appDbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates an existing refresh token with new token data.
        /// Generates a new refresh token while maintaining the same ID.
        /// </summary>
        /// <param name="refreshToken">The original refresh token to update.</param>
        /// <param name="updatedRefreshToken">Output parameter containing the newly generated token.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <exception cref="ArgumentException">Thrown when the refresh token doesn't exist.</exception>
        public void UpdateAsync(RefreshToken refreshToken, out RefreshToken updatedRefreshToken, CancellationToken cancellationToken = default)
        {
            // Find existing token by user ID and token ID
            var item = appDbContext.RefreshTokens.FirstOrDefault(e => e.UserId == refreshToken.UserId && e.Id == refreshToken.Id);

            if (item == null)
            {
                throw new ArgumentException("This refresh token does not exist.");
            }

            // Generate new refresh token with the same ID
            updatedRefreshToken = tokenService.GenerateRefreshToken(item.User);

            // Update token properties with new values
            item.Token = updatedRefreshToken.Token;
            item.Expires = updatedRefreshToken.Expires;

            // Save changes to database
            appDbContext.RefreshTokens.Update(item);
            appDbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Removes a specific refresh token from the database.
        /// </summary>
        /// <param name="refreshToken">The refresh token to remove.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <exception cref="InvalidOperationException">Thrown when the token is not found.</exception>
        public async Task RemoveAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            // Find token or throw exception if not found
            var item = appDbContext.RefreshTokens
                                        .FirstOrDefault(e => e.UserId == refreshToken.UserId && e.Token == refreshToken.Token)
                                        ?? throw new InvalidOperationException("Interaction element not found.");

            // Remove token and save changes
            appDbContext.RefreshTokens.Remove(item);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Removes a refresh token based on token string and user ID.
        /// If the token doesn't exist, no action is taken.
        /// </summary>
        /// <param name="token">The token string to remove.</param>
        /// <param name="userId">The user ID associated with the token.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        public async Task RemoveAsync(string token, Guid userId, CancellationToken cancellationToken = default)
        {
            // Find token by token string and user ID
            var item = appDbContext.RefreshTokens.FirstOrDefault(e => e.Token == token && e.UserId == userId);

            // Remove token if found (no exception if not found)
            if (item != null)
            {
                appDbContext.RefreshTokens.Remove(item);
                await appDbContext.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Validates whether a refresh token exists and is valid for the user.
        /// </summary>
        /// <param name="refreshToken">The refresh token to validate.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>True if the token exists and is valid, otherwise false.</returns>
        public bool IsValidRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken = default)
            => appDbContext.RefreshTokens.FirstOrDefault(e => e.UserId == refreshToken.UserId && e.Id == refreshToken.Id) != null;

        /// <summary>
        /// Retrieves a refresh token by token string and user ID.
        /// Automatically removes expired tokens and cleans up expired tokens for the user.
        /// </summary>
        /// <param name="token">The token string to retrieve.</param>
        /// <param name="userId">The user ID associated with the token.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>The valid refresh token, or null if not found or expired.</returns>
        public async Task<RefreshToken?> GetRefreshTokenAsync(string token, Guid userId, CancellationToken cancellationToken = default)
        {
            // Find refresh token by token string and user ID
            var refreshToken = appDbContext.RefreshTokens.FirstOrDefault(e => e.Token == token && e.UserId == userId);

            // If token not found or expired, remove it and return null
            if (refreshToken == null)
            {
                return null;
            }
            else if (refreshToken.IsExpired)
            {
                await RemoveAsync(refreshToken, cancellationToken);
                return null;
            }

            // Clean up any expired tokens for this user
            await RemoveExpiredTokensAsync(refreshToken.UserId, cancellationToken);

            return refreshToken;
        }

        /// <summary>
        /// Checks if a refresh token already exists in the database.
        /// </summary>
        /// <param name="refreshToken">The refresh token to check.</param>
        /// <returns>True if the token exists, otherwise false.</returns>
        public async Task<bool> IsExistRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
            => await appDbContext.RefreshTokens.AnyAsync(e => e.Token == refreshToken.Token
                                                              && e.UserId == refreshToken.UserId
                                                              && e.IsActive
                                                              && e.Expires <= DateTime.Now,
                                                              cancellationToken);

        /// <summary>
        /// Removes all expired refresh tokens for a specific user.
        /// </summary>
        /// <param name="userId">The user ID whose expired tokens should be removed</param>
        private async Task RemoveExpiredTokensAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            // Find all expired tokens for the user
            var item = appDbContext.RefreshTokens.Where(e => e.UserId == userId && DateTime.Now > e.Expires).ToList();

            // Exit if no expired tokens found
            if (item.Count == 0)
            {
                return;
            }

            // Remove all expired tokens and save changes
            appDbContext.RefreshTokens.RemoveRange(item);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

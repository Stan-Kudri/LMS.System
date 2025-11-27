using System;
using System.Collections.Generic;
using System.Text;
using LMS.System.Domain.Model.Users;
using LMS.System.Infrastructure.Authenticates.Model;

namespace LMS.System.Infrastructure.Service.Validation
{
    /// <summary>
    /// Service for validating user data.
    /// </summary>
    public interface IValidationService
    {
        /// <summary>
        /// Validates user request synchronously.
        /// </summary>
        /// <param name="userRequest">User data entered.</param>
        /// <returns>Validation result.</returns>
        ValidationResultModel Validate(UserRequest userRequest);

        /// <summary>
        /// Validates user data asynchronously.
        /// </summary>
        /// <param name="userRequest">User data entered.</param>
        /// <returns>Validation result.</returns>
        Task<ValidationResultModel> ValidateUserAsync(UserRequest userRequest);

        /// <summary>
        /// Gets only validation errors.
        /// </summary>
        /// <param name="userRequest">User data entered.</param>
        /// <returns>Errors.</returns>
        List<string?> GetValidationErrors(UserRequest userRequest);
    }
}

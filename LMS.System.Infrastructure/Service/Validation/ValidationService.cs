using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using LMS.System.Domain.Model.Users;
using LMS.System.Infrastructure.Authenticates.Model;

namespace LMS.System.Infrastructure.Service.Validation
{
    /// <summary>
    /// Implements user validation using DataAnnotations.
    /// </summary>
    public class ValidationService : IValidationService
    {
        /// <summary>
        /// Validates user request using DataAnnotations.
        /// </summary>
        /// <param name="userRequest">Input: User data object containing properties to validate.</param>
        /// <returns>Output: Validation result indicating success/failure and error messages.</returns>
        public async Task<ValidationResultModel> ValidateUserAsync(UserRequest userRequest)
            => await Task.Run(() => Validate(userRequest));

        /// <summary>
        /// Validates user request synchronously.
        /// </summary>
        /// <param name="userRequest">User data entered.</param>
        /// <returns>Validation result.</returns>
        public ValidationResultModel Validate(UserRequest userRequest)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(userRequest);
            bool isValid = Validator.TryValidateObject(userRequest, context, validationResults, true);

            return new ValidationResultModel
            {
                IsValid = isValid,
                Errors = validationResults.Select(validResult => validResult.ErrorMessage).Where(e => !string.IsNullOrEmpty(e)).ToList(),
            };
        }

        /// <summary>
        /// Gets only validation errors.
        /// </summary>
        /// <param name="userRequest">User data entered.</param>
        /// <returns>Errors.</returns>
        public List<string?> GetValidationErrors(UserRequest userRequest)
            => Validate(userRequest).Errors;
    }
}

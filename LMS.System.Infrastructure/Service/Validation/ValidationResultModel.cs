using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.System.Infrastructure.Service.Validation
{
    /// <summary>
    /// Represents validation result.
    /// </summary>
    public class ValidationResultModel
    {
        /// <summary>
        /// Indicates if validation passed.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// List of validation errors.
        /// </summary>
        public List<string?> Errors { get; set; } = new();
    }
}

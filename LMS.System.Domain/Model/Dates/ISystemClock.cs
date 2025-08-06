using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.System.Domain.Model.Dates
{
    /// <summary>
    /// Interface for get the current date.
    /// </summary>
    public interface ISystemClock
    {
        /// <summary>
        ///  Current date.
        /// </summary>
        DateTime UtcNow { get; }
    }
}

using Ardalis.SmartEnum;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.System.Domain.Extension
{
    /// <summary>
    /// Extension SmartEnum.
    /// </summary>
    public static class SmartEnumExtension
    {
        /// <summary>
        /// Has conversion SmartEnum<typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Entity.</typeparam>
        /// <param name="type">Builder configuration.</param>
        public static void SmartEnumNameConversion<T>(this PropertyBuilder<T> type)
            where T : SmartEnum<T> => type.HasConversion(x => x.Name, x => SmartEnum<T>.FromName(x, false));
    }
}

using LMS.System.Domain.Enums;
using LMS.System.Domain.Extension;
using LMS.System.Domain.Model.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.System.Domain.DB.Configuration
{
    /// <summary>
    /// User configuration model.
    /// </summary>
    public sealed class UserConfiguration : EntityBaseConfiguration<User>
    {
        /// <summary>
        /// Configure User.
        /// </summary>
        /// <param name="builder">Entity Type Builder.</param>
        protected override void ConfigureModel(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");
            builder.HasIndex(e => e.Email).IsUnique();
            builder.Property(e => e.Email).IsRequired().HasColumnName("email").HasMaxLength(128);
            builder.Property(e => e.PasswordHash).IsRequired().HasColumnName("password_hash").HasMaxLength(128);
            builder.Property(e => e.FirstName).IsRequired().HasColumnName("first_name").HasMaxLength(128);
            builder.Property(e => e.LastName).IsRequired().HasColumnName("last_name").HasMaxLength(128);
            builder.Property(e => e.UserRole).IsRequired()
                                             .HasColumnName("role")
                                             .HasDefaultValue(UserRole.Student)
                                             .SmartEnumNameConversion();
        }
    }
}

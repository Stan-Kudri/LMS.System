using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.System.Domain.Model.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.System.Domain.DB.Configuration
{
    /// <summary>
    /// Refresh token configuration for database mapping.
    /// </summary>
    public sealed class RefreshTokenConfiguration : EntityBaseConfiguration<RefreshToken>
    {
        /// <summary>
        /// Configure RefreshToken entity.
        /// </summary>
        /// <param name="builder">Entity Type Builder.</param>
        protected override void ConfigureModel(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_token");
            builder.Property(rt => rt.Token).IsRequired()
                                          .HasColumnName("token")
                                          .HasMaxLength(256);
            builder.Property(rt => rt.Expires).IsRequired().HasColumnName("expires");
            builder.Property(rt => rt.IsRevoked).IsRequired()
                                              .HasColumnName("is_revoked")
                                              .HasDefaultValue(false);
            builder.Property(rt => rt.UserId).IsRequired().HasColumnName("user_id");
            builder.HasOne(rt => rt.User).WithMany(user => user.RefreshTokens)
                                         .HasForeignKey(rt => rt.UserId)
                                         .OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(rt => rt.Token).IsUnique();
            builder.HasIndex(rt => rt.UserId);
            builder.HasIndex(rt => rt.Expires); // To search for expired tokens.
            builder.HasIndex(rt => rt.IsRevoked); // To search for revoked tokens.
            builder.HasIndex(rt => new { rt.Expires, rt.IsRevoked }); // Composite index.
            builder.HasIndex(rt => rt.CreatedAt); // To clear by creation time.
        }
    }
}

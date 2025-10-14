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
            builder.Property(e => e.Token).IsRequired()
                                          .HasColumnName("token")
                                          .HasMaxLength(256);
            builder.Property(e => e.Expires).IsRequired().HasColumnName("expires");
            builder.Property(e => e.IsRevoked).IsRequired()
                                              .HasColumnName("is_revoked")
                                              .HasDefaultValue(false);
            builder.Property(e => e.UserId).IsRequired().HasColumnName("user_id");
            builder.HasOne(rt => rt.User).WithMany(e => e.RefreshTokens)
                                         .HasForeignKey(rt => rt.UserId)
                                         .OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(e => e.Token).IsUnique();
            builder.HasIndex(e => e.UserId);
        }
    }
}

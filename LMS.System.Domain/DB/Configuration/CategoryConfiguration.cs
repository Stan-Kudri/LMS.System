using LMS.System.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.System.Domain.DB.Configuration
{
    /// <summary>
    /// Category configuration.
    /// </summary>
    /// <typeparam name="T">Category.</typeparam>
    public sealed class CategoryConfiguration : EntityBaseConfiguration<Category>
    {
        /// <summary>
        /// Configure category.
        /// </summary>
        /// <param name="builder">Builder DB.</param>
        protected override void ConfigureModel(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("category");
            builder.HasIndex(e => e.Name).IsUnique();
            builder.Property(e => e.Name).IsRequired()
                                         .HasMaxLength(128)
                                         .HasColumnName("name");
        }
    }
}

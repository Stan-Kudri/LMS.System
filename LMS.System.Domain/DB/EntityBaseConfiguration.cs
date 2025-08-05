using LMS.System.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.System.Domain.DB
{
    /// <summary>
    /// Base entity configuration.
    /// </summary>
    /// <typeparam name="T">Model configuration.</typeparam>
    public abstract class EntityBaseConfiguration<T> : IEntityTypeConfiguration<T>
        where T : Entity
    {
        /// <summary>
        /// Configure model.
        /// </summary>
        /// <param name="builder">Builder DB.</param>
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id")
                                       .ValueGeneratedOnAdd();
            builder.Property(e => e.CreatedAt).HasColumnName("created_at")
                                              .HasDefaultValueSql("GETDATE()")
                                              .ValueGeneratedOnAddOrUpdate();
            builder.Property(e => e.UpdatedAt).HasColumnName("update_at")
                                              .HasDefaultValueSql("GETDATE()")
                                              .ValueGeneratedOnAddOrUpdate();
            ConfigureModel(builder);
        }

        /// <summary>
        /// Configure model.
        /// </summary>
        /// <param name="builder">Entity type builder.</param>
        protected abstract void ConfigureModel(EntityTypeBuilder<T> builder);
    }
}

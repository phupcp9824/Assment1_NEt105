using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models;

namespace WebApi.Configuration
{
    public class ComboConfig : IEntityTypeConfiguration<Combo>
    {
        public void Configure(EntityTypeBuilder<Combo> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Picture)
                .HasMaxLength(255);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.HasMany(x => x.OrderDetails)
                .WithOne(x => x.Combo)
                .HasForeignKey(x => x.ComboId);

        }
    }
}

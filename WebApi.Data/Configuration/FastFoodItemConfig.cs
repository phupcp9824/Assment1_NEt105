using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models;

namespace WebApi.Configuration
{
    public class FastFoodItemConfig : IEntityTypeConfiguration<FastFoodItem>
    {
        public void Configure(EntityTypeBuilder<FastFoodItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.HasMany(x => x.OrderDetails)
                .WithOne(x => x.FastFoodItem)
                .HasForeignKey(x => x.FastFoodItemId);
        }
    }
}

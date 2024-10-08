using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models;

namespace WebApi.Configuration
{
    public class OrderDetailConfig : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Combo)
                .WithMany(x => x.OrderDetails)
                .HasForeignKey(x => x.IdCombo)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.FastFoodItem)
                .WithMany(x => x.OrderDetails)
                .HasForeignKey(x => x.IdFood)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderDetails)
                .HasForeignKey(x => x.IdOrder)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

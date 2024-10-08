using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class InvoiceConfig : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(x => x.InvoiceId);

            builder.Property(x => x.Invoice_Date)
                .IsRequired();

            builder.Property(x => x.Total_Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasOne(x => x.Order)
                .WithOne(x => x.Invoice)  
                .HasForeignKey<Invoice>(x => x.Order_id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Invoice) 
                .HasForeignKey(x => x.User_id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

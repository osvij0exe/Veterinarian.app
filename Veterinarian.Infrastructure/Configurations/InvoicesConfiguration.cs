using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Invoices;
using Veterinaria.Domain.Entities.Users;

namespace Veterinarian.Application.Configurations
{
    public class InvoicesConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("invoices");

            builder.HasKey(i => i.Id);
            builder.Property(i => i.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(i => i.UserId)
                .HasMaxLength(500);

            builder.Property(i => i.PaymentMethod);
            builder.Property(i => i.Paid)
                .IsRequired();
            builder.Property(i => i.MedicalConsultationId)
                .IsRequired();


            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(i => i.UserId);
        }
    }
}

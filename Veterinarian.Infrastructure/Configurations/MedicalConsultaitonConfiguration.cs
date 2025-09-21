using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.MedicalConsultations;
using Veterinaria.Domain.Entities.MedicalConsultationUsers;

namespace Veterinarian.Application.Configurations
{
    public class MedicalConsultaitonConfiguration : IEntityTypeConfiguration<MedicalConsultation>
    {
        public void Configure(EntityTypeBuilder<MedicalConsultation> builder)
        {
            builder.ToTable("medicalConsultations");
            builder.HasKey(m => m.Id);


            builder.Property(m => m.AppointmentDate)
                .IsRequired();
            builder.Property(m => m.AppointmentEnd)
                .IsRequired();
            builder.Property(m => m.MedicalTreatMent)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(m => m.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(m => m.PetId)
                .IsRequired();
            builder.Property(m => m.VetId)
                .IsRequired();

            //skip property
            builder.HasMany(m => m.Users)
                .WithMany()
                .UsingEntity<MedicalConsultationUser>();

        }
    }
}

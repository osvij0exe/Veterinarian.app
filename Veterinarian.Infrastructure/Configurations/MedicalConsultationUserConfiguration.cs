using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.MedicalConsultations;
using Veterinaria.Domain.Entities.MedicalConsultationUsers;
using Veterinaria.Domain.Entities.Users;

namespace Veterinarian.Infrastructure.Configurations
{
    public class MedicalConsultationUserConfiguration : IEntityTypeConfiguration<MedicalConsultationUser>
    {
        public void Configure(EntityTypeBuilder<MedicalConsultationUser> builder)
        {
            builder.ToTable("MedicalConsultationUser");
            builder.HasKey(mc => mc.Id);

            builder.HasOne<User>()
                .WithMany(u => u.MedicalConsutlationUser)
                .HasForeignKey(mc => mc.UserId);

            builder.HasOne<MedicalConsultation>()
                .WithMany(u => u.MedicalConsultationUsers)
                .HasForeignKey(mc => mc.MedcialConsultationId);
        }
    }
}

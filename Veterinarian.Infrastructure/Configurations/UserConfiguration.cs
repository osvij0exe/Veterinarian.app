using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.MedicalConsultationUsers;
using Veterinaria.Domain.Entities.Users;

namespace Veterinarian.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(h => h.Id).HasMaxLength(500);

            builder.Property(u => u.Email).HasMaxLength(300);
            builder.Property(u => u.IdentityId).HasMaxLength(500);

            builder.Property(u => u.Name).HasMaxLength(100);

            builder.HasIndex(user => user.Email).IsUnique();
            builder.HasIndex(user => user.IdentityId).IsUnique();

            //skip property
            builder.HasMany(u => u.MedicalConsultations)
                .WithMany()
                .UsingEntity<MedicalConsultationUser>();

        }
    }
}

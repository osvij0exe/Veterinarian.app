using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Users;
using Veterinaria.Domain.Entities.Vets;

namespace Veterinarian.Application.Configurations
{
    public class VetConfiguration : IEntityTypeConfiguration<Vet>
    {
        public void Configure(EntityTypeBuilder<Vet> builder)
        {
            builder.ToTable("Veterinarians");
            builder.HasKey(v => v.Id);

            builder.Property(v => v.UserId)
                .HasMaxLength(500);

            builder.Property(v => v.GivenName)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(v => v.FamilyName)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(v => v.ProfessionalId)
                .IsRequired();
            builder.Property(v => v.Contact)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(v => v.SpecialityId)
                .IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(user => user.UserId);

        }
    }
}

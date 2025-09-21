using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Owners;
using Veterinaria.Domain.Entities.PetOwners;

namespace Veterinarian.Application.Configurations
{
    public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.ToTable("owners");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.UserId)
                .HasMaxLength(500);

            builder.Property(o => o.GivenName)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(o => o.FamilyName)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property( o => o.Contact)
                .HasMaxLength(20)
                .IsRequired();
            builder.Property(o => o.Email)
                .IsRequired();

            //skip porperty
            builder.HasMany(o => o.Pets)
                .WithMany()
                .UsingEntity<PetOwner>();
        }
    }
}

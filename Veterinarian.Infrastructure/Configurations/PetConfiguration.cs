using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.PetOwners;
using Veterinaria.Domain.Entities.Pets;

namespace Veterinarian.Application.Configurations
{
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("pets");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.UserId)
                .HasMaxLength(500);

            builder.Property(p => p.Name)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(p => p.Specie)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(p => p.Breed)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(p => p.GenderStatus)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(p => p.BirhtDate)
                .IsRequired();
            //skip porperty
            builder.HasMany(p => p.Owners)
                .WithMany()
                .UsingEntity<PetOwner>();
        }
    }
}

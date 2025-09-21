using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Owners;
using Veterinaria.Domain.Entities.PetOwners;
using Veterinaria.Domain.Entities.Pets;

namespace Veterinarian.Infrastructure.Configurations
{
    public class PetOwnerConfiguration : IEntityTypeConfiguration<PetOwner>
    {
        public void Configure(EntityTypeBuilder<PetOwner> builder)
        {
            builder.ToTable("petOwners");
            builder.HasKey(po => po.Id);

            builder.HasOne<Pet>()
                   .WithMany(p => p.PetsOwner)
                   .HasForeignKey(po => po.PetId);

            builder.HasOne<Owner>()
                   .WithMany(o => o.PetsOwnner)
                   .HasForeignKey(po => po.OwnerId);
        }
    }
}

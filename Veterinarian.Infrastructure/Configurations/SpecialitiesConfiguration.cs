using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Sécialities;

namespace Veterinarian.Application.Configurations
{
    public class SpecialitiesConfiguration : IEntityTypeConfiguration<Speciality>
    {
        public void Configure(EntityTypeBuilder<Speciality> builder)
        {
            builder.ToTable("Specialities");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .HasMaxLength(250)
                .IsRequired();
        }
    }
}

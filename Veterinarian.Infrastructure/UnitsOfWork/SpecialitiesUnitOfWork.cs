using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Sécialities;

namespace Veterinarian.Application.UnitsOfWork
{
    public class SpecialitiesUnitOfWork : UnitOfWork
    {
        public SpecialitiesUnitOfWork(ApplicationDbContext dbContext,
            ISpecialityRepository specialityRepository) 
            : base(dbContext)
        {
            SpecialityRepository = specialityRepository;
        }

        public ISpecialityRepository SpecialityRepository { get; }
    }
}

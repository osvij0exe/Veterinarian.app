using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Sécialities;

namespace Veterinarian.Application.Repositories
{
    public class SpecialitiesRepository : RepositoryBase<Speciality>, ISpecialityRepository
    {
        public SpecialitiesRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}

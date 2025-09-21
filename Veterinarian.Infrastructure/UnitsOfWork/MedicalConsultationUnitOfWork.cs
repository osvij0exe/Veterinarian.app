using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.MedicalConsultations;
using Veterinaria.Domain.Entities.Pets;
using Veterinaria.Domain.Entities.Users;
using Veterinaria.Domain.Entities.Vets;
using Veterinarian.Infrastructure.ServicesFiles;

namespace Veterinarian.Application.UnitsOfWork
{
    public class MedicalConsultationUnitOfWork : UnitOfWork
    {
        public MedicalConsultationUnitOfWork(
            ApplicationDbContext dbContext,
            IMedicalConsultationRepository medicalConsultationRepository,
            IPetsRepository petsRepository,
            IVetsRepository vetsRepository,
            IUserContext userContext) 
            : base(dbContext)
        {
            MedicalConsultationRepository = medicalConsultationRepository;
            PetsRepository = petsRepository;
            VetsRepository = vetsRepository;
            UserContext = userContext;
        }

        public IUserContext UserContext;
        public IMedicalConsultationRepository MedicalConsultationRepository { get; }
        public IPetsRepository PetsRepository { get; }
        public IVetsRepository VetsRepository { get; }
    }
}

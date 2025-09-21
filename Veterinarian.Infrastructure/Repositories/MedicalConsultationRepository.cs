using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Common;
using Veterinaria.Domain.Entities.MedicalConsultations;
using Veterinarian.Infrastructure.Common;

namespace Veterinarian.Application.Repositories
{
    public class MedicalConsultationRepository : RepositoryBase<MedicalConsultation>, IMedicalConsultationRepository
    {
        public readonly ApplicationDbContext _dbContext;
        public MedicalConsultationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MedicalConsultation>> GetConsultationByVetIdAsync(Guid id)
        {
            var consultation = await _dbContext
                .Set<MedicalConsultation>()
                .Where(c => c.VetId.Equals(id))
                .ToListAsync();
            return consultation;
        }

        public async Task<List<MedicalConsultation>> GetConsultationByPetIdAsync(Guid id)
        {
            var consultation = await _dbContext
                .Set<MedicalConsultation>()
                .Where(c => c.PetId == id)
                .ToListAsync();
            return consultation;
        }

        public async Task<List<MedicalConsultation>> SearchConsultationByPetOrVetAsync(string? search)
        {
            var query = _dbContext.Set<MedicalConsultation>().AsQueryable();

            var consultation = await query
                .Where(q => q.Pet!.Name.Contains(search ?? string.Empty)
                    || q.Pet.Breed.Contains(search ?? string.Empty)
                    || q.Pet.Specie.Contains(search ?? string.Empty)
                    || q.MedicalTreatMent.Contains(search ?? string.Empty)
                    || q.Vet!.FamilyName.Contains(search ?? string.Empty)
                    || q.Vet.GivenName.Contains(search ?? string.Empty)
                    || q.Vet.Email.Contains(search ?? string.Empty)
                    || q.Vet.ProfessionalId.Contains(search ?? string.Empty)
                    || q.Vet.Contact.Contains(search ?? string.Empty)
                    || q.Vet.Speciality.Name.Contains(search ?? string.Empty))
                .Include(q => q.Vet).ThenInclude(v => v!.Speciality)
                .Include(q => q.Pet)
                .AsNoTracking()
                .ToListAsync();
            return consultation;

        }
        public async Task<PaginationResult<MedicalConsultation>> SearchConsultationByPetOrVetAsync(string? search, int page, int pageSize)
        {
            var query = _dbContext.Set<MedicalConsultation>().AsQueryable().Where(q => q.Pet!.Name.Contains(search ?? string.Empty)
                    || q.Pet.Breed.Contains(search ?? string.Empty)
                    || q.Pet.Specie.Contains(search ?? string.Empty)
                    || q.MedicalTreatMent.Contains(search ?? string.Empty)
                    || q.Vet!.FamilyName.Contains(search ?? string.Empty)
                    || q.Vet.GivenName.Contains(search ?? string.Empty)
                    || q.Vet.Email.Contains(search ?? string.Empty)
                    || q.Vet.ProfessionalId.Contains(search ?? string.Empty)
                    || q.Vet.Contact.Contains(search ?? string.Empty)
                    || q.Vet.Speciality.Name.Contains(search ?? string.Empty))
                .Include(q => q.Vet).ThenInclude(v => v!.Speciality)
                .Include(q => q.Pet)
                .AsNoTracking();

            var response = await PaginationProvider<MedicalConsultation>.CreateAsync(query, page, pageSize);
            return response;

        }

        public async Task<List<MedicalConsultation>> SearchConsultationsByDateAsync(DateTime? search)
        {
            var query = _dbContext.Set<MedicalConsultation>().AsQueryable();

            return await query.Where(q => q.AppointmentDate.Equals(search)).ToListAsync();
        }

    }
}

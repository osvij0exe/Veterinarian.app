using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.Common;

namespace Veterinaria.Domain.Entities.MedicalConsultations
{
    public interface IMedicalConsultationRepository : IReposirotyBase<MedicalConsultation>
    {
        Task<List<MedicalConsultation>> SearchConsultationsByDateAsync(DateTime? search);
        Task<List<MedicalConsultation>> GetConsultationByPetIdAsync(Guid id);
        Task<List<MedicalConsultation>> GetConsultationByVetIdAsync(Guid id);
        Task<List<MedicalConsultation>> SearchConsultationByPetOrVetAsync(string? search);
        Task<PaginationResult<MedicalConsultation>> SearchConsultationByPetOrVetAsync(string? search, int page, int pageSize);
    }
}

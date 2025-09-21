using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinarian.Application.Common;

namespace Veterinarian.Application.MedicalConsultations
{
    public interface IMedicalConsultationServices
    {
        Task<Result> CreateAsync(MedicalConsultationRequest request);
        Task<Result> DeleteAsync(Guid id);
        Task<Result> UpdateAsync(Guid id,MedicalConsultationRequest resources);
        Task<Result<MedicalConsulationResponse>> GetByIdAsync(Guid id);
        Task<Result<List<MedicalConsulationResponse>>> GetAllAsync();
        Task<Result<List<MedicalConsulationResponse>>> SearchConsultationByBetOrVetAsync(string? search);
        Task<Result<PaginationResultDto<MedicalConsulationResponse>>> SearchConsultationByBetOrVetAsync(string? search,int page = 1, int pageSize = 5);
    }
}

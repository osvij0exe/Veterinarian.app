using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Veterinarian.Application.MedicalConsultations;

namespace Veterinarian.Api.Controllers
{
    [ApiController]
    [Route("Api/MedicalConsultations")]
    public class MedicalConsultationController : ControllerBase
    {
        private readonly IMedicalConsultationServices _medicalConsultationServices;

        public MedicalConsultationController(IMedicalConsultationServices medicalConsultationServices)
        {
            _medicalConsultationServices = medicalConsultationServices;
        }

        [HttpGet("GetAllMediaclConsultation")]
        public async Task<IActionResult> GetAllMedicalConsultations()
        {
            var consultations = await _medicalConsultationServices.GetAllAsync();
            return Ok(consultations.Value);
        }

        [HttpGet("GetMedicalConsultation/{id}")]
        public async Task<IActionResult> GetMedicalConsultationById(Guid id)
        {
            var consultation = await _medicalConsultationServices.GetByIdAsync(id);
            return consultation.IsSuccess ? Ok(consultation.Value) : NotFound(consultation.Error);
        }

        [HttpGet("SearchConsultationByPetOrVet")]
        public async Task<IActionResult> SearchConsultationByPetOrVet(string? search,int page = 1, int pageSize = 5)
        {
            var consultation = await _medicalConsultationServices.SearchConsultationByBetOrVetAsync(search,page,pageSize);

            return Ok(consultation.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedicalConsultation([FromBody] MedicalConsultationRequest request,
            IValidator<MedicalConsultationRequest> validator)
        {

            ValidationResult validationResult= await validator.ValidateAsync(request);

            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var result = await _medicalConsultationServices.CreateAsync(request);
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalConsultation(Guid id)
        {
            var result = await _medicalConsultationServices.DeleteAsync(id);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicalConsultation(Guid id, [FromBody] MedicalConsultationRequest request,
            IValidator<MedicalConsultationRequest> validator)
        {
            ValidationResult validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }
            var result = await _medicalConsultationServices.UpdateAsync(id, request);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }

    }
}

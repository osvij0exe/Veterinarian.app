using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Veterinaria.Domain.Entities.Users;
using Veterinarian.Application.MedicalConsultations;

namespace Veterinarian.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/medicalConsultations")]
    public class MedicalConsultationController : ControllerBase
    {
        private readonly IMedicalConsultationServices _medicalConsultationServices;
        private readonly IUserContext _userContext;

        public MedicalConsultationController(IMedicalConsultationServices medicalConsultationServices,
            IUserContext userContext)
        {
            _medicalConsultationServices = medicalConsultationServices;
            _userContext = userContext;
        }

        [Authorize(Roles =$"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember}")]
        [HttpGet("getAllMediaclConsultation")]
        public async Task<IActionResult> GetAllMedicalConsultations(CancellationToken cancellationToken)
        {

            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            var consultations = await _medicalConsultationServices.GetAllAsync();
            return Ok(consultations.Value);
        }

        [Authorize(Roles = $"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember}")]
        [HttpGet("getMedicalConsultation/{id}")]
        public async Task<IActionResult> GetMedicalConsultationById(Guid id, CancellationToken cancellationToken)
        {
            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }
            var consultation = await _medicalConsultationServices.GetByIdAsync(id);
            return consultation.IsSuccess ? Ok(consultation.Value) : NotFound(consultation.Error);
        }

        [Authorize(Roles = $"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember}")]
        [HttpGet("searchConsultationByPetOrVet")]
        public async Task<IActionResult> SearchConsultationByPetOrVet(string? search,CancellationToken cancellationToken,int page = 1, int pageSize = 5)
        {
            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            var consultation = await _medicalConsultationServices.SearchConsultationByBetOrVetAsync(search,page,pageSize);

            return Ok(consultation.Value);
        }


        [Authorize(Roles = $"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember}")]
        [HttpPost]
        public async Task<IActionResult> CreateMedicalConsultation([FromBody] MedicalConsultationRequest request,
            IValidator<MedicalConsultationRequest> validator,
             CancellationToken cancellationToken)
        {
            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            ValidationResult validationResult= await validator.ValidateAsync(request);

            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var result = await _medicalConsultationServices.CreateAsync(request);
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        [Authorize(Roles = $"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalConsultation(Guid id, CancellationToken cancellationToken)
        {
            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }
            var result = await _medicalConsultationServices.DeleteAsync(id);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }

        [Authorize(Roles = $"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicalConsultation(Guid id, [FromBody] MedicalConsultationRequest request,
            IValidator<MedicalConsultationRequest> validator,CancellationToken cancellationToken)
        {
            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }
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

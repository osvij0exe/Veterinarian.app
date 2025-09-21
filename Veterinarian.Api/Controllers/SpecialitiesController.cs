using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Veterinarian.Application.Specialities;

namespace Veterinarian.Api.Controllers
{
    [ApiController]
    [Route("Api/Specialities")]
    public class SpecialitiesController : ControllerBase
    {
        private readonly ISpecialitiesServices _specialitiesServices;

        public SpecialitiesController(ISpecialitiesServices specialitiesServices)
        {
            _specialitiesServices = specialitiesServices;
        }

        [HttpGet("GetAllSpecialities")]
        public async Task<IActionResult> GetAllSpecialities()
        {
            var specialities = await _specialitiesServices.GetAllAsync();
            return Ok(specialities.Value);
        }

        [HttpGet("GetSpeciality/{id}")]
        public async Task<IActionResult> GetSpecialityById(Guid id)
        {
            var speciality = await _specialitiesServices.GetByIdAsync(id);
            return speciality.IsSuccess ? Ok(speciality.Value) : NotFound(speciality.Error); 

        }

        [HttpPost]
        public async Task<IActionResult> CreateSpeciality(
            SpecialitiesRequest request, 
            IValidator<SpecialitiesRequest> validator)
        {

            ValidationResult validationResult = await validator.ValidateAsync(request);
            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }


            var speciality = await _specialitiesServices.CreateAsync(request);

            return speciality.IsSuccess ? Ok(speciality) : BadRequest(speciality.Error);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpeciality(Guid id, SpecialitiesRequest request,IValidator<SpecialitiesRequest> validator)
        {

            ValidationResult validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var speciality = await _specialitiesServices.UpdateAsync(id, request);
            return speciality.IsSuccess ? Ok(speciality) : BadRequest(speciality.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpeciality(Guid id)
        {
            var speciality = await _specialitiesServices.DeleteAsync(id);
            return speciality.IsSuccess ? Ok(speciality) : NotFound(speciality.Error);
        }


    }
}

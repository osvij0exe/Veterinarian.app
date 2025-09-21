using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Veterinarian.Application.Vets;

namespace Veterinarian.Api.Controllers
{
    [ApiController]
    [Route("Api/Vets")]
    public class VetsController : ControllerBase
    {
        private readonly IVetServices _vetServices;

        public VetsController(IVetServices vetServices)
        {
            _vetServices = vetServices;
        }

        [HttpGet("GetVets")]
        public async Task<IActionResult> GetVets()
        {
            var vets = await _vetServices.GetAllAsync();
            return Ok(vets.Value);
        }

        [HttpGet("GetVet/{id}")]
        public async Task<IActionResult> GetVetById(Guid id)
        {
            var vet = await _vetServices.GetByIdAsync(id);
            return vet.IsSuccess ? Ok(vet.Value) : NotFound(vet.Error);
        }
        [HttpGet("SearchVet")]
        public async Task<IActionResult> SerchVets([FromQuery] string? search, int page = 1, int pageSize = 5)
        {
            var vets = await _vetServices.SearchVetAsync(search,page,pageSize);
            return Ok(vets.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVet([FromBody] VetRequest request,
            IValidator<VetRequest> validator)
        {
            ValidationResult validationResult = await validator.ValidateAsync(request);

            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var response = await _vetServices.CreateAndRegisterAsync(request);
            return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVet(Guid id)
        {
            var response = await _vetServices.DeleteAsync(id);
            return response.IsSuccess ? NoContent() : NotFound(response.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVet(Guid id, [FromBody] VetRequest request
            ,IValidator<VetRequest> validator)
        {
            ValidationResult validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var response = await _vetServices.UpdateAsync(id, request);
            return response.IsSuccess ? NoContent() : NotFound(response.Error);
        }       



    }
}

using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Veterinarian.Application.Owners;

namespace Veterinarian.Api.Controllers
{
    [ApiController]
    [Route("Api/Owners")]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerServices _ownerServices;

        public OwnersController(IOwnerServices ownerServices)
        {
            _ownerServices = ownerServices;
        }

        [HttpGet("GetAllOwners")]
        public async Task<IActionResult> GetAllOwners()
        {
            var owners = await _ownerServices.GetAllAsync();
            return Ok(owners.Value);
        }

        [HttpGet("GetOwner/{id}")]
        public async Task<IActionResult> GetOwnerById(Guid id)
        {
            var owner = await _ownerServices.GetByIdAsync(id);
            return owner.IsSuccess ? Ok(owner.Value) : NotFound(owner.Error);
        }

        [HttpGet("SearchOwner")]
        public async Task<IActionResult> SearchOwner(string? search,int page, int pageSize)
        {
            var response = await _ownerServices.SearchOnwers(search,page,pageSize);

            return Ok(response.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOwner([FromBody] OwnerRequest request,
            IValidator<OwnerRequest> validator)
        {
            ValidationResult validationResult = await validator.ValidateAsync(request);

            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }


            var result = await _ownerServices.CreateAndRegisterAsync(request);
            return result.IsSuccess ?NoContent() : BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOwner(Guid id)
        {
            var result = await _ownerServices.DeleteAsync(id);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOwner(Guid id, [FromBody] OwnerUpdateRequest request,
            IValidator<OwnerUpdateRequest> validator)
        {

            ValidationResult validationResult = await validator.ValidateAsync(request);

            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var result = await _ownerServices.UpdateAsync(id, request);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }   

    }
}

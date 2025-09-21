using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Veterinarian.Application.Pets;

namespace Veterinarian.Api.Controllers
{
    [ApiController]
    [Route("Api/Pets")]
    public class PetsController : ControllerBase
    {
        private readonly IPetServices _petServices;

        public PetsController(IPetServices petServices)
        {
            _petServices = petServices;
        }

        [HttpGet("GetPets")]
        public async Task<IActionResult> GetPets()
        {
            var pets = await _petServices.GetAllAsync();
            return Ok(pets.Value);
        }

        [HttpGet("GetPet/{id}")]
        public async Task<IActionResult> GetPetById(Guid id)
        {
            var pet = await _petServices.GetByIdAsync(id);
            
            return pet.IsSuccess ? Ok(pet.Value) : NotFound(pet.Error);
        }
        [HttpGet("searchPet")]
        public async Task<IActionResult> SearchPets([FromQuery] string? search, int page = 1,int pageSize = 5)
        {
            var pets = await _petServices.SearchPetAsync(search,page,pageSize);
            return Ok(pets.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePet(PetRequest request, 
            IValidator<PetRequest> validator)
        {
            ValidationResult validationResult = await validator.ValidateAsync(request);

            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var pet = await _petServices.CreateAsync(request);
            return pet.IsSuccess ? NoContent() : BadRequest(pet.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet(Guid id)
        {
            var pet = await _petServices.DeleteAsync(id);
            return pet.IsSuccess ? NoContent() : NotFound(pet.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePet(Guid id, PetUpdateRequest request,
            IValidator<PetUpdateRequest> validator)
        {
            ValidationResult validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }
            var pet = await _petServices.UpdateAsync(id, request);
            return pet.IsSuccess ? NoContent() : BadRequest(pet.Error);
        }
    }
}

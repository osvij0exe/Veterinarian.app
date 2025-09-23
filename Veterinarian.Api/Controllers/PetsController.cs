using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Veterinaria.Domain.Entities.Users;
using Veterinarian.Application.Pets;

namespace Veterinarian.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/pets")]
    public class PetsController : ControllerBase
    {
        private readonly IPetServices _petServices;
        private readonly IUserContext _userContext;

        public PetsController(IPetServices petServices,
            IUserContext userContext)
        {
            _petServices = petServices;
            _userContext = userContext;
        }

        [Authorize(Roles =$"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember}")]
        [HttpGet("getPets")]
        public async Task<IActionResult> GetPets(CancellationToken cancellationToken)
        {


            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            var pets = await _petServices.GetAllAsync();
            return Ok(pets.Value);
        }
        [Authorize(Roles =$"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember}")]
        [HttpGet("getPet/{id}")]
        public async Task<IActionResult> GetPetById(Guid id,CancellationToken cancellationToken)
        {

            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            var pet = await _petServices.GetByIdAsync(id);
            
            return pet.IsSuccess ? Ok(pet.Value) : NotFound(pet.Error);
        }

        [Authorize(Roles = $"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember}")]
        [HttpGet("searchPet")]
        public async Task<IActionResult> SearchPets([FromQuery] string? search,CancellationToken cancellationToken, int page = 1,int pageSize = 5)
        {

            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }


            var pets = await _petServices.SearchPetAsync(search,page,pageSize);
            return Ok(pets.Value);
        }

        [Authorize(Roles = $"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember}")]
        [HttpPost]
        public async Task<IActionResult> CreatePet(PetRequest request, 
            IValidator<PetRequest> validator,CancellationToken cancellationToken)
        {

            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }


            ValidationResult validationResult = await validator.ValidateAsync(request);

            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var pet = await _petServices.CreateAsync(request);
            return pet.IsSuccess ? NoContent() : BadRequest(pet.Error);
        }

        [Authorize(Roles = $"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet(Guid id, CancellationToken cancellationToken)
        {
                
            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }


            var pet = await _petServices.DeleteAsync(id);
            return pet.IsSuccess ? NoContent() : NotFound(pet.Error);
        }


        [Authorize(Roles = $"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePet(Guid id, PetUpdateRequest request,
            IValidator<PetUpdateRequest> validator, CancellationToken cancellationToken)
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
            var pet = await _petServices.UpdateAsync(id, request);
            return pet.IsSuccess ? NoContent() : BadRequest(pet.Error);
        }
    }
}

using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Domain.Entities.Users;
using Veterinarian.Application.AuthServices;
using Veterinarian.Application.Users;
using Veterinarian.Application.Vets;

namespace Veterinarian.Api.Controllers
{
    [ApiController]
    [Route("Api/Vets")]
    public class VetsController : ControllerBase
    {
        private readonly IVetServices _vetServices;
        private readonly IUserManagerServices _userManagerServices;
        private readonly RoleManager<IdentityRole> _roleManager;

        public VetsController(IVetServices vetServices,
            IUserManagerServices userManagerServices,
            RoleManager<IdentityRole> roleManager)
        {
            _vetServices = vetServices;
            _userManagerServices = userManagerServices;
            _roleManager = roleManager;
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

            //identity User identityDbContext
            var identityUser = new IdentityUser
            {
                Email = request.Email,
                UserName = request.Email
            };

            IdentityResult identityResult = await _userManagerServices.IdentityUserRegister(identityUser, request.Password);

            if (!identityResult.Succeeded)
            {
                var extensions = new Dictionary<string, object?>
                {
                    {
                        "error",
                        identityResult.Errors.ToDictionary(e => e.Code,e => e.Description)
                    }
                };
                return Problem(
                    detail: "Unable to register user, please try again",
                    statusCode: StatusCodes.Status400BadRequest,
                    extensions: extensions);
            }

            var existRole = await _roleManager.RoleExistsAsync(Role.VetMember);
            if (!existRole)
            {
                _userManagerServices.RemoveIdentityUserAsinc(identityUser);
                return Problem(
                    detail: "Unable to register user, The role provided does not exist",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var addtoRole = await _userManagerServices.AddToRoleAsync(identityUser, Role.VetMember);

            if (!addtoRole.Succeeded)
            {
                var extensions = new Dictionary<string, object?>
                {
                    {
                        "error",
                        identityResult.Errors.ToDictionary(e => e.Code,e => e.Description)
                    }
                };
                return Problem(
                    detail: "Unable to register user, please try again",
                    statusCode: StatusCodes.Status400BadRequest,
                    extensions: extensions);

            }



            var response = await _vetServices.CreateAndRegisterAsync(request,identityUser);
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

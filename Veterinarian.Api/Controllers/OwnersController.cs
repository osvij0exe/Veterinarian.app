using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Domain.Entities.Users;
using Veterinarian.Application.AuthServices;
using Veterinarian.Application.Owners;
using Veterinarian.Application.Users;

namespace Veterinarian.Api.Controllers
{
    [ApiController]
    [Route("Api/Owners")]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerServices _ownerServices;
        private readonly IUserManagerServices _userManagerServices;
        private readonly RoleManager<IdentityRole> _roleManager;

        public OwnersController(IOwnerServices ownerServices,
            IUserManagerServices userManagerServices,
            RoleManager<IdentityRole> roleManager)
        {
            _ownerServices = ownerServices;
            _userManagerServices = userManagerServices;
            _roleManager = roleManager;
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
        public async Task<IActionResult> SearchOwner(string? search,int page = 1, int pageSize = 5)
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

            var existRole = await _roleManager.RoleExistsAsync(Role.Owner);
            if (!existRole)
            {
                _userManagerServices.RemoveIdentityUserAsinc(identityUser);
                return Problem(
                    detail: "Unable to register user, The role provided does not exist",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var addtoRole = await _userManagerServices.AddToRoleAsync(identityUser, Role.Owner);

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



            var result = await _ownerServices.CreateAndRegisterAsync(request, identityUser);
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

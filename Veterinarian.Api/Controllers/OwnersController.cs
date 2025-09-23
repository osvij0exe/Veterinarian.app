using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Veterinaria.Domain.Entities.Users;
using Veterinarian.Application.AuthServices;
using Veterinarian.Application.Owners;
using Veterinarian.Application.Users;

namespace Veterinarian.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/owners")]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerServices _ownerServices;
        private readonly IUserManagerServices _userManagerServices;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserContext _userContext;

        public OwnersController(IOwnerServices ownerServices,
            IUserManagerServices userManagerServices,
            RoleManager<IdentityRole> roleManager,
            IUserContext userContext)
        {
            _ownerServices = ownerServices;
            _userManagerServices = userManagerServices;
            _roleManager = roleManager;
            _userContext = userContext;
        }

        [Authorize(Roles =$"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember}")]
        [HttpGet("getAllOwners")]
        public async Task<IActionResult> GetAllOwners(CancellationToken cancellationToken)
        {

            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            var owners = await _ownerServices.GetAllAsync();
            return Ok(owners.Value);
        }

        [Authorize(Roles =$"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember},{Role.Owner}")]
        [HttpGet("getOwner/{id}")]
        public async Task<IActionResult> GetOwnerById(Guid id,CancellationToken cancellationToken)
        {
            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }


            var owner = await _ownerServices.GetByIdAsync(id);
            return owner.IsSuccess ? Ok(owner.Value) : NotFound(owner.Error);
        }

        [Authorize(Roles =$"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember}")]
        [HttpGet("searchOwner")]
        public async Task<IActionResult> SearchOwner(string? search,CancellationToken cancellationToken, int page = 1, int pageSize = 5)
        {

            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            var response = await _ownerServices.SearchOnwers(search,page,pageSize);

            return Ok(response.Value);
        }

        [Authorize(Roles =$"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember}")]
        [HttpPost]
        public async Task<IActionResult> CreateOwner([FromBody] OwnerRequest request,
            IValidator<OwnerRequest> validator,CancellationToken cancellationToken)
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

        [Authorize(Roles =$"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOwner(Guid id, CancellationToken cancellationToken)
        {
            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            var result = await _ownerServices.DeleteAsync(id);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }

        [Authorize(Roles =$"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOwner(Guid id, [FromBody] OwnerUpdateRequest request,
            IValidator<OwnerUpdateRequest> validator,CancellationToken cancellationToken)
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

            var result = await _ownerServices.UpdateAsync(id, request);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }   

    }
}

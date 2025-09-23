using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.Users;
using Veterinarian.Application.AuthServices;
using Veterinarian.Application.Users;
using Veterinarian.Application.UserServices;
using Veterinarian.Application.Vets;

namespace Veterinarian.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/vets")]
    public class VetsController : ControllerBase
    {
        private readonly IVetServices _vetServices;
        private readonly IUserManagerServices _userManagerServices;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserContext _userContext;

        public VetsController(IVetServices vetServices,
            IUserManagerServices userManagerServices,
            RoleManager<IdentityRole> roleManager,
            IUserContext userContext)
        {
            _vetServices = vetServices;
            _userManagerServices = userManagerServices;
            _roleManager = roleManager;
            _userContext = userContext;
        }

        [Authorize(Roles = $"{Role.AuxiliaryMember},{Role.Admin}")]
        [HttpGet("getVets")]
        public async Task<IActionResult> GetVets(CancellationToken cancellationToken)
        {

            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode:StatusCodes.Status401Unauthorized);
            }


            var vets = await _vetServices.GetAllAsync();
            return Ok(vets.Value);
        }

        [Authorize(Roles =$"{Role.Admin},{Role.AuxiliaryMember},{Role.VetMember}")]
        [HttpGet("getVet/{id}")]
        public async Task<IActionResult> GetVetById(Guid id, CancellationToken cancellationToken)
        {

            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }


            var vet = await _vetServices.GetByIdAsync(id);
            return vet.IsSuccess ? Ok(vet.Value) : NotFound(vet.Error);
        }

        [Authorize(Roles = $"{Role.Admin},{Role.AuxiliaryMember}")]
        [HttpGet("searchVet")]
        public async Task<IActionResult> SerchVets([FromQuery] string? search,CancellationToken cancellationToken, int page = 1, int pageSize = 5)
        {

            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }


            var vets = await _vetServices.SearchVetAsync(search,page,pageSize);
            return Ok(vets.Value);
        }

        [Authorize(Roles = $"{Role.Admin},{Role.AuxiliaryMember}")]
        [HttpPost]
        public async Task<IActionResult> CreateVet([FromBody] VetRequest request,
            IValidator<VetRequest> validator, CancellationToken cancellationToken)
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

        [Authorize(Roles = $"{Role.Admin},{Role.AuxiliaryMember}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVet(Guid id, CancellationToken cancellationToken)
        {
            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Problem(
                    detail: "Unauthorized",
                    statusCode: StatusCodes.Status401Unauthorized);
            }


            var response = await _vetServices.DeleteAsync(id);
            return response.IsSuccess ? NoContent() : NotFound(response.Error);
        }

        [Authorize(Roles = $"{Role.VetMember}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVet(Guid id, [FromBody] VetRequest request
            ,IValidator<VetRequest> validator,CancellationToken cancellationToken)
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

            var response = await _vetServices.UpdateAsync(id, request,CurrentUserId);
            return response.IsSuccess ? NoContent() : NotFound(response.Error);
        }       



    }
}

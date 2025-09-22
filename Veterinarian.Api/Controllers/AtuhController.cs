using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Domain.Entities.Users;
using Veterinarian.Application.AuthServices;
using Veterinarian.Application.Users;
using Veterinarian.Security.Token;

namespace Veterinarian.Api.Controllers
{
    [ApiController]
    [Route("Api/auth")]
    [AllowAnonymous]
    public class AtuhController : ControllerBase
    {
        private readonly IApplicationUserServices _identityUserServices;
        private readonly IUserManagerServices _userManagerServices;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AtuhController(IApplicationUserServices identityUserServices,
            IUserManagerServices userManagerServices,
            RoleManager<IdentityRole> roleManager)
        {
            _identityUserServices = identityUserServices;
            _userManagerServices = userManagerServices;
            _roleManager = roleManager;
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserDto registerUserDto)
        {

            //identity User identityDbContext
            var identityUser = new IdentityUser
            {
                Email = registerUserDto.Email,
                UserName = registerUserDto.Email
            };

            IdentityResult identityResult = await _userManagerServices.IdentityUserRegister(identityUser,registerUserDto.Password);

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

            var existRole = await _roleManager.RoleExistsAsync(Role.AuxiliaryMember);
            if (!existRole)
            {
                 _userManagerServices.RemoveIdentityUserAsinc(identityUser);
                return Problem(
                    detail: "Unable to register user, The role provided does not exist",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var addtoRole = await _userManagerServices.AddToRoleAsync(identityUser,Role.AuxiliaryMember);

            if(!addtoRole.Succeeded)
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


            var user = await _identityUserServices.Register(registerUserDto,identityUser);

            return user.IsSuccess ? Ok(user.Value) : BadRequest(user.Error);

        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserDto loginUserDto)
        {
            var accessToken = await _identityUserServices.Login(loginUserDto);

            return accessToken.IsSuccess ? Ok(accessToken.Value) : Unauthorized(accessToken.Error);
        }
        [HttpPost("refresh")]
        public async Task<ActionResult<AccessTokenDto>> Refresh(RefreshTokenDto refreshTokenDto)
        {
            var acessToken = await _identityUserServices.RefreshTokenProvider(refreshTokenDto);

            return acessToken.IsSuccess ? Ok(acessToken.Value) : Unauthorized(acessToken.Error);

        }

    }
}

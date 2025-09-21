using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Veterinarian.Application.Users;
using Veterinarian.Security.Token;

namespace Veterinarian.Api.Controllers
{
    [ApiController]
    [Route("Api/auth")]
    [AllowAnonymous]
    public class AtuhController : ControllerBase
    {
        private readonly IIdentityUserServices _identityUserServices;

        public AtuhController(IIdentityUserServices identityUserServices)
        {
            _identityUserServices = identityUserServices;
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserDto registerUserDto)
        {
            var user = await _identityUserServices.Register(registerUserDto);

            return user.IsSuccess ? Ok(user) : BadRequest(user.Error);

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

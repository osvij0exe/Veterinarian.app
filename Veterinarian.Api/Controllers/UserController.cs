using Microsoft.AspNetCore.Mvc;
using Veterinarian.Application.UserServices;

namespace Veterinarian.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUserById(string id,CancellationToken cancellationToken)
        {
            var user = await _userServices.GetUserByIdAsync(id,cancellationToken);

            return user.IsSuccess ? Ok(user) : Problem(
                detail:user.Error.NameError,
                statusCode:int.Parse(user.Error.Code));

        }

        [HttpGet("me")]
        public async Task<ActionResult<UserResponse>> CurrentUserAsync(CancellationToken cancellationToken)
        {
            var user = await _userServices.GetCurrentUser(cancellationToken);

            return user.IsSuccess ? Ok(user) : Problem(
                detail: user.Error.NameError,
                statusCode: int.Parse(user.Error.Code));

        }



    }
}

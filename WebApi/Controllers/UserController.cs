using Application.Features.UsersInfo.Commands;
using Application.Features.UsersInfo.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        [HttpPost("login-customer")]
        public async Task<IActionResult> LoginCustomer(LoginCustomerCommand request) => Ok(await Mediator.Send(request));

        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser(RegisterUserQuery request) => Ok(await Mediator.Send(request));

        [HttpPost("user-info")]
        public async Task<IActionResult> GetUserInfo(GetUserInfoQuery request) => Ok(await Mediator.Send(request));
    }
}

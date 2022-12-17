using Application.Features.LoanApplicationsInfo.Commands;
using Application.Features.LoanApplicationsInfo.Queries;
using Application.Features.StatusApplicationsInfo.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanApplicationsController : BaseApiController
    {
        [Authorize]
        [HttpPost("user-loans")]
        public async Task<IActionResult> GetUserLoanApplications(GetUserLoanApplicationsQuery request) => Ok(await Mediator.Send(request));

        [Authorize]
        [HttpPost("user-loan-by-id")]
        public async Task<IActionResult> GetUserLoanApplicationById(GetUserLoanApplicationByIdQuery request) => Ok(await Mediator.Send(request));

        [HttpGet("status-types")]
        public async Task<IActionResult> GetApplicationStatusTypes([FromQuery] GetApplicationStatusTypesQuery request) => Ok(await Mediator.Send(request));

        [HttpGet("loan-types")]
        public async Task<IActionResult> GetApplicationLoanTypes([FromQuery] GetApplicationLoanTypesQuery request) => Ok(await Mediator.Send(request));

        [Authorize]
        [HttpPost("insert-loan")]
        public async Task<IActionResult> InsertUserLoanApplication(InsertUserLoanApplicationCommand request) => Ok(await Mediator.Send(request));

        [HttpPost("update-loan")]
        public async Task<IActionResult> UpdateUserLoanApplication(UpdateUserLoanApplicationCommand request) => Ok(await Mediator.Send(request));

        [HttpPost("delete-loan")]
        public async Task<IActionResult> DeleteUserLoanApplication(DeleteUserLoanApplicationCommand request) => Ok(await Mediator.Send(request));
    }
}

using AdminDashboard.BusinessLogicOrchestrators.LoginOrchestrator;
using AdminDashboard.Models.JsonRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GravitationalTest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginOrchestrator Orchestrator;

        public LoginController(ILoginOrchestrator orchestrator)
        {
            Orchestrator = orchestrator;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticationRequest model)
        {
            string token = Orchestrator.Authenticate(model.Username, model.Password);

            return Ok(token);
        }
    }
}

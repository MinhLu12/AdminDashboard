using AdminDashboard.BusinessLogicOrchestrators.AccountOrchestrator;
using AdminDashboard.Models.JsonRequests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AdminDashboard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountOrchestrator Orchestrator;

        public AccountController(IAccountOrchestrator orchestrator)
        {
            Orchestrator = orchestrator;
        }

        /// <summary>
        /// Gets an account by its id
        /// </summary>
        /// <param name="id">The unique identifier for the account</param>
        /// <returns>The account</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var retrievedAccount = await Orchestrator.GetAccountBy(id);

            return Ok(retrievedAccount);
        }

        /// <summary>
        /// Gets all users of an account
        /// </summary>
        /// <param name="accountId">The unique identifier for the account</param>
        /// <returns>The account users</returns>
        [HttpGet("{accountId:guid}/Users")]
        public async Task<IActionResult> GetUsers(Guid accountId)
        {
            var accounts = await Orchestrator.GetAccountUsers(accountId);

            return Ok(accounts);
        }

        /// <summary>
        /// Adds a user to an account
        /// </summary>
        /// <param name="accountId">Id of the account</param>
        /// <param name="userId">Id of the user</param>
        /// <returns></returns>
        [HttpPut("{accountId:guid}/User/{userId:guid}")]
        public async Task RegisterUser(Guid accountId, Guid userId)
        {
            await Orchestrator.RegisterUserToAccount(accountId, userId);
        }

        /// <summary>
        /// Upgrades an account plan
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{accountId:guid}")]
        public async Task<IActionResult> UpgradePlan(Guid accountId, [FromBody]UpgradePlanRequest request)
        {
            await Orchestrator.UpgradePlan(accountId, request);

            return Ok();
        }

        /// <summary>
        /// Creates an account with the specified plan type
        /// </summary>
        /// <param name="request">The plan type associated to the new account</param>
        /// <returns>The id of the created account</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateAccountRequest request)
        {
            return Ok(await Orchestrator.CreateAccount(request));
        }
    }
}

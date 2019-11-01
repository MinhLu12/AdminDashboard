using AdminDashboard.BusinessLogicOrchestrators.AccountOrchestrator;
using AdminDashboard.Models.JsonRequests;
using AdminDashboard.Repositories.AccountRepository;
using AdminDashboard.Validators.Accounts;
using AdminDashboard.Validators.Plans;
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
        private readonly IAccountRepository Repository;

        private readonly IAccountValidator Validator;
        private readonly IPlanValidator PlanValidator;

        public AccountController(IAccountOrchestrator orchestrator,
            IAccountRepository repository,
            IAccountValidator validator,
            IPlanValidator planValidator)
        {
            Orchestrator = orchestrator;
            Repository = repository;
            Validator = validator;
            PlanValidator = planValidator;
        }

        /// <summary>
        /// Gets an account by its id
        /// </summary>
        /// <param name="id">The unique identifier for the account</param>
        /// <returns>The account</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount(Guid id)
        {
            if (!await Validator.DoesAccountExist(id))
                return NotFound();

            return Ok(await Orchestrator.GetAccountBy(id));
        }

        /// <summary>
        /// Gets all users of an account
        /// </summary>
        /// <param name="accountId">The unique identifier for the account</param>
        /// <returns>The account users</returns>
        [HttpGet("{accountId:guid}/Users")]
        public async Task<IActionResult> GetAccountUsers(Guid accountId)
        {
            if (!await Validator.DoesAccountExist(accountId))
                return NotFound();

            return Ok(await Orchestrator.GetAccountUsers(accountId));
        }

        /// <summary>
        /// Adds a user to an account
        /// </summary>
        /// <param name="accountId">Id of the account</param>
        /// <param name="userId">Id of the user</param>
        /// <returns></returns>
        [HttpPut("{accountId:guid}/User/{userId:guid}")]
        public async Task<IActionResult> RegisterUserToAccount(Guid accountId, Guid userId)
        {
            if (!await Validator.DoesAccountExist(accountId))
                return NotFound();

            if (await Validator.DoesAccountAlreadyHaveUser(accountId, userId))
                return BadRequest();

            if (await Validator.HasAccountReachedMaximumNumberOfUsers(accountId))
                return BadRequest(false);

            await Repository.AddUserAndAccountRelationship(accountId, userId);

            return Ok(true);
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
            if (!await Validator.DoesAccountExist(accountId))
                return NotFound();

            if (!PlanValidator.IsPlanValid(request.Plan))
                return BadRequest();

            if (!await PlanValidator.IsValidUpgradeTransition(accountId, request.Plan))
                return BadRequest();

            return Ok(await Repository.UpgradePlan(accountId, request.Plan));
        }

        /// <summary>
        /// Creates an account with the specified plan type
        /// </summary>
        /// <param name="request">The plan type associated to the new account</param>
        /// <returns>The id of the created account</returns>
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody]CreateAccountRequest request)
        {
            if (!PlanValidator.IsPlanValid(request.Plan))
                return BadRequest();

            return Ok(await Orchestrator.CreateAccount(request));
        }
    }
}
